using SystemsRx.Systems.Conventional;
using SystemsRx.Tests.Models;

namespace SystemsRx.Tests.SystemsRx.Handlers.Helpers
{
    public interface ITestMultiReactToEvent : IReactToEventSystem<ComplexObject>, IReactToEventSystem<int>
    {
        
    }
}