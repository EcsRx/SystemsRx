using System;
using System.Collections;
using System.Collections.Generic;
using SystemsRx.MicroRx;
using SystemsRx.MicroRx.Operators;
using SystemsRx.MicroRx.Subjects;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.ReactiveData.Dictionaries
{
    [Serializable]
    public class ReactiveDictionary<TKey, TValue> : IReactiveDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        [NonSerialized] private bool _isDisposed = false;

        private readonly Dictionary<TKey, TValue> _inner;

        public ReactiveDictionary()
        {
            _inner = new Dictionary<TKey, TValue>();
        }

        public ReactiveDictionary(IEqualityComparer<TKey> comparer)
        {
            _inner = new Dictionary<TKey, TValue>(comparer);
        }

        public ReactiveDictionary(Dictionary<TKey, TValue> innerDictionary)
        {
            _inner = innerDictionary;
        }

        public TValue this[TKey key]
        {
            get => _inner[key];

            set
            {
                if (TryGetValue(key, out var oldValue))
                {
                    _inner[key] = value;
                    _dictionaryReplace?.OnNext(new DictionaryReplaceEvent<TKey, TValue>(key, oldValue, value));
                }
                else
                {
                    _inner[key] = value;
                    _dictionaryAdd?.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
                    _countChanged?.OnNext(Count);
                }
            }
        }

        public int Count => _inner.Count;
        public IEnumerable<TKey> Keys => _inner.Keys;
        public IEnumerable<TValue> Values => _inner.Values;

        public void Add(TKey key, TValue value)
        {
            _inner.Add(key, value);

            _dictionaryAdd?.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
            _countChanged?.OnNext(Count);
        }

        public void Clear()
        {
            var beforeCount = Count;
            _inner.Clear();

            _collectionReset?.OnNext(Unit.Default);
            if (beforeCount <= 0) { return; }
            _countChanged?.OnNext(Count);
        }

        public bool Remove(TKey key)
        {
            if (!_inner.TryGetValue(key, out var oldValue)) return false;
            
            var isSuccessRemove = _inner.Remove(key);
            if (!isSuccessRemove) 
            {return isSuccessRemove;}

            _dictionaryRemove?.OnNext(new DictionaryRemoveEvent<TKey, TValue>(key, oldValue));
            _countChanged?.OnNext(Count);
            return isSuccessRemove;
        }

        public bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        private void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
        {
            if (subject == null) { return; }
            try
            {
                subject.OnCompleted();
            }
            finally
            {
                subject.Dispose();
                subject = null;
            }
        }

        #region IDisposable Support

        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) { return; }
            
            if (disposing)
            {
                DisposeSubject(ref _countChanged);
                DisposeSubject(ref _collectionReset);
                DisposeSubject(ref _dictionaryAdd);
                DisposeSubject(ref _dictionaryRemove);
                DisposeSubject(ref _dictionaryReplace);
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


        #region Observe

        [NonSerialized] private Subject<int> _countChanged = null;
        public IObservable<int> ObserveCountChanged()
        {
            if (_isDisposed) { return ImmutableEmptyObservable<int>.Instance; }
            return _countChanged ?? (_countChanged = new Subject<int>());
        }

        [NonSerialized] private Subject<Unit> _collectionReset = null;
        public IObservable<Unit> ObserveReset()
        {
            if (_isDisposed) { return ImmutableEmptyObservable<Unit>.Instance; }
            return _collectionReset ?? (_collectionReset = new Subject<Unit>());
        }

        [NonSerialized] private Subject<DictionaryAddEvent<TKey, TValue>> _dictionaryAdd = null;
        public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd()
        {
            if (_isDisposed) { return ImmutableEmptyObservable<DictionaryAddEvent<TKey, TValue>>.Instance; }
            return _dictionaryAdd ?? (_dictionaryAdd = new Subject<DictionaryAddEvent<TKey, TValue>>());
        }

        [NonSerialized] private Subject<DictionaryRemoveEvent<TKey, TValue>> _dictionaryRemove = null;
        public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove()
        {
            if (_isDisposed) { return ImmutableEmptyObservable<DictionaryRemoveEvent<TKey, TValue>>.Instance; }
            return _dictionaryRemove ?? (_dictionaryRemove = new Subject<DictionaryRemoveEvent<TKey, TValue>>());
        }

        [NonSerialized] private Subject<DictionaryReplaceEvent<TKey, TValue>> _dictionaryReplace = null;
        public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace()
        {
            if (_isDisposed) { return ImmutableEmptyObservable<DictionaryReplaceEvent<TKey, TValue>>.Instance; }
            return _dictionaryReplace ?? (_dictionaryReplace = new Subject<DictionaryReplaceEvent<TKey, TValue>>());
        }

        #endregion

        #region implement explicit

        object IDictionary.this[object key]
        {
            get => this[(TKey)key];
            set => this[(TKey)key] = (TValue)value;
        }


        bool IDictionary.IsFixedSize => ((IDictionary)_inner).IsFixedSize;

        bool IDictionary.IsReadOnly => ((IDictionary)_inner).IsReadOnly;

        bool ICollection.IsSynchronized => ((IDictionary)_inner).IsSynchronized;

        ICollection IDictionary.Keys => ((IDictionary)_inner).Keys;

        object ICollection.SyncRoot => ((IDictionary)_inner).SyncRoot;

        ICollection IDictionary.Values => ((IDictionary)_inner).Values;


        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).IsReadOnly;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _inner.Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => _inner.Values;

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)_inner).Contains(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary)_inner).CopyTo(array, index);
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey)key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue v;
            if (!TryGetValue(item.Key, out v)) { return false; }
            if (!EqualityComparer<TValue>.Default.Equals(v, item.Value)) { return false; }
            Remove(item.Key);
            return true;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_inner).GetEnumerator();
        }

        #endregion
    }
}