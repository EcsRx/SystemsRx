using System;

namespace SystemsRx.MicroRx.Events
{
    public interface IMessageReceiver
    {
        /// <summary>
        /// Subscribe typed message.
        /// </summary>
        IObservable<T> Receive<T>();
    }
}