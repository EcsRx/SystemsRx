using System;
using System.Threading;

namespace SystemsRx.MicroRx.Observers
{
    public class SubscribeObserver<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        private int _isStopped = 0;

        public SubscribeObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            if (_isStopped == 0)
            { _onNext(value); }
        }

        public void OnError(Exception error)
        {
            if (Interlocked.Increment(ref _isStopped) == 1)
            { _onError(error); }
        }


        public void OnCompleted()
        {
            if (Interlocked.Increment(ref _isStopped) == 1)
            { _onCompleted(); }
        }
    }
}