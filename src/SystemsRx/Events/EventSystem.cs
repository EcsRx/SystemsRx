using System;
using System.Threading.Tasks;
using SystemsRx.MicroRx.Events;
using SystemsRx.Threading;

namespace SystemsRx.Events
{
    public class EventSystem : IEventSystem
    {
        public IMessageBroker MessageBroker { get; }
        public IThreadHandler ThreadHandler { get; }

        public EventSystem(IMessageBroker messageBroker, IThreadHandler threadHandler)
        {
            MessageBroker = messageBroker;
            ThreadHandler = threadHandler;
        }

        public void Publish<T>(T message)
        { MessageBroker.Publish(message); }

        public void PublishAsync<T>(T message)
        { ThreadHandler.Run(() => MessageBroker.Publish(message)); }

        public IObservable<T> Receive<T>()
        { return MessageBroker.Receive<T>(); }

    }
}