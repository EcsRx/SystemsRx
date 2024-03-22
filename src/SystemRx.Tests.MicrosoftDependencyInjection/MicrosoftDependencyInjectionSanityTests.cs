using System.Linq;
using SystemsRx.Executor.Handlers;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Infrastructure.Modules;
using SystemsRx.Infrastructure.MicrosoftDependencyInjection;
using SystemsRx.Scheduling;
using Xunit;

namespace SystemsRx.Tests.MicrosoftDependencyInjection
{
    public class MicrosoftDependencyInjectionSanityTests
    {
        [Fact]
        public void should_bind_and_resolve()
        {
            var diRegistry = new MicrosoftDependencyRegistry();
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