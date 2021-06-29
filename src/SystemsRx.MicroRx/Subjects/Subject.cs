using System;
using SystemsRx.MicroRx.Disposables;
using SystemsRx.MicroRx.Observers;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.MicroRx.Subjects
{
    public sealed class Subject<T> : ISubject<T>, IDisposable
    {
        private readonly object _observerLock = new object();

        private bool _isStopped;
        private bool _isDisposed;
        private Exception _lastError;
        private IObserver<T> _outObserver = EmptyObserver<T>.Instance;

        public bool HasObservers => !(_outObserver is EmptyObserver<T>) && !_isStopped && !_isDisposed;

        public void OnCompleted()
        {
            IObserver<T> old;
            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (_isStopped) { return; }

                old = _outObserver;
                _outObserver = EmptyObserver<T>.Instance;
                _isStopped = true;
            }

            old.OnCompleted();
        }

        public void OnError(Exception error)
        {
            if (error == null)
            { throw new ArgumentNullException(nameof(error)); }
            
            IObserver<T> old;
            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (_isStopped) { return; }

                old = _outObserver;
                _outObserver = EmptyObserver<T>.Instance;
                _isStopped = true;
                _lastError = error;
            }

            old.OnError(error);
        }

        public void OnNext(T value)
        {
            _outObserver.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) 
            { throw new ArgumentNullException(nameof(observer)); }

            var ex = default(Exception);

            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (!_isStopped)
                {
                    if (_outObserver is ListObserver<T> listObserver)
                    { _outObserver = listObserver.Add(observer); }
                    else
                    {
                        var current = _outObserver;
                        if (current is EmptyObserver<T>)
                        { _outObserver = observer; }
                        else
                        { _outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new[] { current, observer })); }
                    }

                    return new Subscription(this, observer);
                }

                ex = _lastError;
            }

            if (ex != null)
            { observer.OnError(ex); }
            else
            { observer.OnCompleted(); }

            return Disposable.Empty;
        }

        public void Dispose()
        {
            lock (_observerLock)
            {
                _isDisposed = true;
                _outObserver = DisposedObserver<T>.Instance;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            { throw new ObjectDisposedException(""); }
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        private class Subscription : IDisposable
        {
            private readonly object _gate = new object();
            private Subject<T> _parent;
            private IObserver<T> _unsubscribeTarget;

            public Subscription(Subject<T> parent, IObserver<T> unsubscribeTarget)
            {
                _parent = parent;
                _unsubscribeTarget = unsubscribeTarget;
            }

            public void Dispose()
            {
                lock (_gate)
                {
                    if (_parent == null) { return; }
                    
                    lock (_parent._observerLock)
                    {
                        if (_parent._outObserver is ListObserver<T> listObserver)
                        { _parent._outObserver = listObserver.Remove(_unsubscribeTarget); }
                        else
                        { _parent._outObserver = EmptyObserver<T>.Instance; }

                        _unsubscribeTarget = null;
                        _parent = null;
                    }
                }
            }
        }
    }
}