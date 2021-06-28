using SystemsRx.Executor.Handlers.Conventional;
using SystemsRx.Systems;
using SystemsRx.Systems.Conventional;
using NSubstitute;
using Xunit;

namespace SystemsRx.Tests.SystemsRx.Handlers
{
    public class ManualSystemHandlerTests
    {
        [Fact]
        public void should_correctly_handle_systems()
        {
            var teardownSystemHandler = new ManualSystemHandler();
            
            var fakeMatchingSystem = Substitute.For<IManualSystem>();
            var fakeNonMatchingSystem1 = Substitute.For<ISystem>();
            var fakeNonMatchingSystem2 = Substitute.For<ISystem>();
            
            Assert.True(teardownSystemHandler.CanHandleSystem(fakeMatchingSystem));
            Assert.False(teardownSystemHandler.CanHandleSystem(fakeNonMatchingSystem1));
            Assert.False(teardownSystemHandler.CanHandleSystem(fakeNonMatchingSystem2));
        }
        
        [Fact]
        public void should_start_system_when_added_to_handler()
        {
            var mockSystem = Substitute.For<IManualSystem>();
            var systemHandler = new ManualSystemHandler();
            systemHandler.SetupSystem(mockSystem);
            
            mockSystem.Received(1).StartSystem();
        }
        
        [Fact]
        public void should_stop_system_when_added_to_handler()
        {
            var mockSystem = Substitute.For<IManualSystem>();
            var systemHandler = new ManualSystemHandler();
            systemHandler.DestroySystem(mockSystem);
            
            mockSystem.Received(1).StopSystem();
        }
    }
}