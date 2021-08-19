using System.Threading.Tasks;
using SystemsRx.Events;
using SystemsRx.Events.Process;

namespace SystemsRx.Extensions
{
    public static class IEventSystemExtensions
    {
        public static Task<T> StartProcess<T>(this IEventSystem eventSystem, IAsyncProcess<T> process)
        { return process.Execute(eventSystem); }
    }
}