using System;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.MicroRx.Observers
{
    public class EmptyObserver<T> : IObserver<T>
    {
        public static readonly EmptyObserver<T> Instance = new EmptyObserver<T>();

        private EmptyObserver()
        {

        }

        public void OnCompleted()
        {
            // Nothing to complete
        }

        public void OnError(Exception error)
        {
            // Nothing to error
        }

        public void OnNext(T value)
        {
            // Nothing to push
        }
    }
}