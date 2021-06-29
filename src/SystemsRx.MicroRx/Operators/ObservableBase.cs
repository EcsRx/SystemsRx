using System;
using SystemsRx.MicroRx.Disposables;

namespace SystemsRx.MicroRx.Operators
{
    public abstract class OperatorObservableBase<T> : IObservable<T>
    {
        private readonly bool _isRequiredSubscribeOnCurrentThread;

        protected OperatorObservableBase(bool isRequiredSubscribeOnCurrentThread)
        {
            _isRequiredSubscribeOnCurrentThread = isRequiredSubscribeOnCurrentThread;
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return _isRequiredSubscribeOnCurrentThread;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var subscription = new SingleAssignmentDisposable();
            subscription.Disposable = SubscribeCore(observer, subscription);
            return subscription;
        }

        protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel);
    }
}