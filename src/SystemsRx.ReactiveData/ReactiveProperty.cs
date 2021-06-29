using System;
using System.Collections.Generic;
using SystemsRx.MicroRx.Disposables;
using SystemsRx.MicroRx.Subjects;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.ReactiveData
{
    /// <summary>
    /// Lightweight property broker.
    /// </summary>
    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private static readonly IEqualityComparer<T> defaultEqualityComparer = EqualityComparer<T>.Default;

        [NonSerialized] private bool _canPublishValueOnSubscribe = false;
        [NonSerialized] private bool _isDisposed = false;
        private T _value = default(T);

        [NonSerialized] private Subject<T> _publisher = null;
        [NonSerialized] private IDisposable _sourceConnection = null;
        [NonSerialized] private Exception _lastException = null;

        protected virtual IEqualityComparer<T> EqualityComparer => defaultEqualityComparer;

        public T Value
        {
            get => _value;
            set
            {
                if (!_canPublishValueOnSubscribe)
                {
                    _canPublishValueOnSubscribe = true;
                    SetValue(value);

                    if (_isDisposed) { return; }
                    
                    var p = _publisher;
                    p?.OnNext(this._value);
                    return;
                }

                if (EqualityComparer.Equals(this._value, value)) { return; }
                {
                    SetValue(value);

                    if (_isDisposed) return;
                    var p = _publisher;
                    p?.OnNext(this._value);
                }
            }
        }

        public bool HasValue => _canPublishValueOnSubscribe;

        public ReactiveProperty()
            : this(default(T))
        {
            // default constructor 'can' publish value on subscribe.
            // because sometimes value is deserialized from UnityEngine.
        }

        public ReactiveProperty(T initialValue)
        {
            SetValue(initialValue);
            _canPublishValueOnSubscribe = true;
        }

        public ReactiveProperty(IObservable<T> source)
        {
            // initialized from source's ReactiveProperty `doesn't` publish value on subscribe.
            // because there ReactiveProperty is `Future/Task/Promise`.

            _canPublishValueOnSubscribe = false;
            _sourceConnection = source.Subscribe(new ReactivePropertyObserver(this));
        }

        public ReactiveProperty(IObservable<T> source, T initialValue)
        {
            _canPublishValueOnSubscribe = false;
            Value = initialValue; // Value set canPublishValueOnSubscribe = true
            _sourceConnection = source.Subscribe(new ReactivePropertyObserver(this));
        }

        protected virtual void SetValue(T value)
        {
            this._value = value;
        }

        public void SetValueAndForceNotify(T value)
        {
            SetValue(value);

            if (_isDisposed) { return; }

            var p = _publisher;
            p?.OnNext(this._value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_lastException != null)
            {
                observer.OnError(_lastException);
                return Disposable.Empty;
            }

            if (_isDisposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            if (_publisher == null)
            {
                // Interlocked.CompareExchange is bit slower, guarantee thread safety is overkill.
                // System.Threading.Interlocked.CompareExchange(ref publisher, new Subject<T>(), null);
                _publisher = new Subject<T>();
            }

            var p = _publisher;
            if (p != null)
            {
                var subscription = p.Subscribe(observer);
                if (_canPublishValueOnSubscribe)
                {
                    observer.OnNext(_value); // raise latest value on subscribe
                }
                return subscription;
            }
            else
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) { return; }
            _isDisposed = true;
            var sc = _sourceConnection;
            if (sc != null)
            {
                sc.Dispose();
                _sourceConnection = null;
            }
            var p = _publisher;
            if (p == null) { return; }
            // when dispose, notify OnCompleted
            try
            {
                p.OnCompleted();
            }
            finally
            {
                p.Dispose();
                _publisher = null;
            }
        }

        public override string ToString()
        {
            return (_value == null) ? "(null)" : _value.ToString();
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        private class ReactivePropertyObserver : IObserver<T>
        {
            readonly ReactiveProperty<T> parent;
            int isStopped = 0;

            public ReactivePropertyObserver(ReactiveProperty<T> parent)
            {
                this.parent = parent;
            }

            public void OnNext(T value)
            {
                parent.Value = value;
            }

            public void OnError(Exception error)
            {
                if (System.Threading.Interlocked.Increment(ref isStopped) != 1) { return; }
                
                parent._lastException = error;
                var p = parent._publisher;
                p?.OnError(error);
                parent.Dispose(); // complete subscription
            }

            public void OnCompleted()
            {
                if (System.Threading.Interlocked.Increment(ref isStopped) != 1) { return; }
                // source was completed but can publish from .Value yet.
                var sc = parent._sourceConnection;
                parent._sourceConnection = null;
                sc?.Dispose();
            }
        }
    }
}