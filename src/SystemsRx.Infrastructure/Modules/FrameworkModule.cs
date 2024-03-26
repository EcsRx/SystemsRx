using SystemsRx.Events;
using SystemsRx.Executor;
using SystemsRx.Executor.Handlers;
using SystemsRx.Executor.Handlers.Conventional;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.MicroRx.Events;
using SystemsRx.Scheduling;
using SystemsRx.Threading;

namespace SystemsRx.Infrastructure.Modules
{
    public class FrameworkModule : IDependencyModule
    {
        public void Setup(IDependencyRegistry registry)
        {
            registry.Bind<IMessageBroker, MessageBroker>();
            registry.Bind<IEventSystem, EventSystem>();
            registry.Bind<IThreadHandler, DefaultThreadHandler>();
            registry.Bind<IConventionalSystemHandler, ManualSystemHandler>();
            registry.Bind<IConventionalSystemHandler, BasicSystemHandler>();
            registry.Bind<IConventionalSystemHandler, ReactToEventSystemHandler>();
            registry.Bind<ISystemExecutor, SystemExecutor>();
            registry.Bind<IUpdateScheduler, DefaultUpdateScheduler>();
            registry.Bind<ITimeTracker>(x => x.ToBoundType(typeof(IUpdateScheduler)));
        }
    }
}