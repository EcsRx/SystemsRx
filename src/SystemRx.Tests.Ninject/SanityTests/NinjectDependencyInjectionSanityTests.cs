using System.Linq;
using SystemsRx.Executor.Handlers;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Infrastructure.Modules;
using SystemsRx.Infrastructure.Ninject;
using SystemsRx.Scheduling;
using Xunit;

namespace SystemsRx.Tests.Ninject.SanityTests
{
    public class NinjectDependencyInjectionSanityTests
    {
        [Fact]
        public void should_bind_and_resolve()
        {
            var diRegistry = new NinjectDependencyRegistry();
            diRegistry.LoadModule<FrameworkModule>();

            var resolver = diRegistry.BuildResolver();
            var systems = resolver.ResolveAll<IConventionalSystemHandler>();
            Assert.NotNull(systems);
            Assert.NotEmpty(systems);
            Assert.Equal(3, systems.Count());
                
            var timeTracker = resolver.Resolve<ITimeTracker>();
            Assert.NotNull(timeTracker);
        }
    }
}