using SystemsRx.Extensions;
using SystemsRx.Systems.Conventional;
using NSubstitute;
using Xunit;

namespace SystemsRx.Tests.SystemsRx
{
    public class ISystemExtensionTests
    {
        [Fact]
        public void should_get_interface_generic_type_from_reactive_data_system()
        {
            var fakeSystem = Substitute.For<IReactToEventSystem<int>>();
            var genericType = fakeSystem.GetGenericDataType(typeof(IReactToEventSystem<>));
            var typesMatch = genericType == typeof(int);
            Assert.True(typesMatch);
        }
    }
}