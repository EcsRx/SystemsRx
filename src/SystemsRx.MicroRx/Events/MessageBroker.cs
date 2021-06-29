using System;
using System.Collections.Generic;
using SystemsRx.MicroRx.Subjects;

/*
 *    This code was taken from UniRx project by neuecc
 *    https://github.com/neuecc/UniRx
 */
namespace SystemsRx.MicroRx.Events
{
    public class MessageBroker : IMessageBroker, IDisposable
    {
        /// <summary>
        /// MessageBroker in Global scope.
        /// </summary>
        public static readonly IMessageBroker Default = new MessageBroker();

        private bool _isDisposed;
        private readonly Dictionary<Type, object> _notifiers = new Dictionary<Type, object>();

        public void Publish<T>(T message)
        {
            object notifier;
            lock (_notifiers)
            {
                if (_isDisposed) { return; }

                if (!_notifiers.TryGetValue(typeof(T), out notifier))
                { return; }
            }
            ((ISubject<T>)notifier).OnNext(message);
        }

        public IObservable<T> Receive<T>()
        {
            object notifier;
            lock (_notifiers)
            {
                if (_isDisposed) 
                { throw new ObjectDisposedException("MessageBroker"); }

                if (_notifiers.TryGetValue(typeof(T), out notifier)) 
                { return ((IObservable<T>) notifier); }
                
                var n = new Subject<T>();
                notifier = n;
                _notifiers.Add(typeof(T), notifier);
            }
            return ((IObservable<T>)notifier);
        }

        public void Dispose()
        {
            lock (_notifiers)
            {
                if (_isDisposed) { return; }
                _isDisposed = true;
                _notifiers.Clear();
            }
        }
    }
}