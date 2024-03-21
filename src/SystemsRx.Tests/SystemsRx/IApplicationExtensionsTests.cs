using System.Collections.Generic;
using System.Linq;
using SystemsRx.Infrastructure;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Systems;
using SystemsRx.Tests.Systems;
using NSubstitute;
using Xunit;

namespace SystemsRx.Tests.SystemsRx
{
    public class IApplicationExtensionsTests
    {
        [Fact]
        public void should_correctly_order_default_systems()
        {

            var lowerThanDefaultPrioritySystem = new LowerThanDefaultPrioritySystem();
            var highPrioritySystem = new HighPrioritySystem();
            var highestPrioritySetupSystem = new HighestPrioritySystem();

            var systemList = new List<ISystem>
            {
                lowerThanDefaultPrioritySystem,
                highPrioritySystem,
                highestPrioritySetupSystem,
            };

            var mockContainer = Substitute.For<IDependencyResolver>();
            var mockApplication = Substitute.For<ISystemsRxApplication>();
            mockContainer.ResolveAll(typeof(ISystem)).Returns(systemList);
            mockApplication.DependencyResolver.Returns(mockContainer);

            var orderedSystems = ISystemsRxApplicationExtensions.GetAllBoundSystems(mockApplication).ToList();

            Assert.Equal(3, orderedSystems.Count);
            Assert.Equal(highestPrioritySetupSystem, orderedSystems[0]);
            Assert.Equal(highPrioritySystem, orderedSystems[1]);
            Assert.Equal(lowerThanDefaultPrioritySystem, orderedSystems[2]);
        }
    }
}