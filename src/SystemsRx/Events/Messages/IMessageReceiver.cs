using R3;

namespace SystemsRx.Events.Messages
{
    public interface IMessageReceiver
    {
        /// <summary>
        /// Subscribe typed message.
        /// </summary>
        Observable<T> Receive<T>();
    }
}