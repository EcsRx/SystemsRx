using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemsRx.MicroRx;
using SystemsRx.MicroRx.Operators;
using SystemsRx.MicroRx.Subjects;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.ReactiveData.Collections
{
    [Serializable]
    public class ReactiveCollection<T> : Collection<T>, IReactiveCollection<T>
    {
        [NonSerialized]
        bool isDisposed = false;

        public ReactiveCollection()
        {

        }

        public ReactiveCollection(IEnumerable<T> collection)
        {
            if (collection == null) 
            { throw new ArgumentNullException(nameof(collection)); }

            foreach (var item in collection)
            { Add(item); }
        }

        public ReactiveCollection(List<T> list) : base(list != null ? new List<T>(list) : null)
        {
        }

        protected override void ClearItems()
        {
            var beforeCount = Count;
            base.ClearItems();

            _collectionReset?.OnNext(Unit.Default);
            if (beforeCount > 0)
            { _countChanged?.OnNext(Count); }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            _collectionAdd?.OnNext(new CollectionAddEvent<T>(index, item));
            _countChanged?.OnNext(Count);
        }

        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            var item = this[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);
            _collectionMove?.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, item));
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);

            _collectionRemove?.OnNext(new CollectionRemoveEvent<T>(index, item));
            _countChanged?.OnNext(Count);
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];
            base.SetItem(index, item);
            _collectionReplace?.OnNext(new CollectionReplaceEvent<T>(index, oldItem, item));
        }


        [NonSerialized] private Subject<int> _countChanged = null;
        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false)
        {
            if (isDisposed) 
            { return ImmutableEmptyObservable<int>.Instance; }

            var subject = _countChanged ?? (_countChanged = new Subject<int>());
            if (notifyCurrentCount)
            {
                subject.OnNext(Count);
            }
            return subject;
        }

        [NonSerialized] private Subject<Unit> _collectionReset = null;
        public IObservable<Unit> ObserveReset()
        {
            if (isDisposed) return ImmutableEmptyObservable<Unit>.Instance;
            return _collectionReset ?? (_collectionReset = new Subject<Unit>());
        }

        [NonSerialized] private Subject<CollectionAddEvent<T>> _collectionAdd = null;
        public IObservable<CollectionAddEvent<T>> ObserveAdd()
        {
            if (isDisposed) return ImmutableEmptyObservable<CollectionAddEvent<T>>.Instance;
            return _collectionAdd ?? (_collectionAdd = new Subject<CollectionAddEvent<T>>());
        }

        [NonSerialized] private Subject<CollectionMoveEvent<T>> _collectionMove = null;
        public IObservable<CollectionMoveEvent<T>> ObserveMove()
        {
            if (isDisposed) return ImmutableEmptyObservable<CollectionMoveEvent<T>>.Instance;
            return _collectionMove ?? (_collectionMove = new Subject<CollectionMoveEvent<T>>());
        }

        [NonSerialized] private Subject<CollectionRemoveEvent<T>> _collectionRemove = null;
        public IObservable<CollectionRemoveEvent<T>> ObserveRemove()
        {
            if (isDisposed) return ImmutableEmptyObservable<CollectionRemoveEvent<T>>.Instance;
            return _collectionRemove ?? (_collectionRemove = new Subject<CollectionRemoveEvent<T>>());
        }

        [NonSerialized] private Subject<CollectionReplaceEvent<T>> _collectionReplace = null;
        public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
        {
            if (isDisposed) return ImmutableEmptyObservable<CollectionReplaceEvent<T>>.Instance;
            return _collectionReplace ?? (_collectionReplace = new Subject<CollectionReplaceEvent<T>>());
        }

        void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
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
                DisposeSubject(ref _collectionReset);
                DisposeSubject(ref _collectionAdd);
                DisposeSubject(ref _collectionMove);
                DisposeSubject(ref _collectionRemove);
                DisposeSubject(ref _collectionReplace);
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        #endregion
    }
}