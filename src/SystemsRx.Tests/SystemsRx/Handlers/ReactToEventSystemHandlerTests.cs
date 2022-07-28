using System;
using System.Linq;
using SystemsRx.Events;
using SystemsRx.Executor.Handlers.Conventional;
using SystemsRx.MicroRx.Subjects;
using SystemsRx.Systems;
using SystemsRx.Systems.Conventional;
using SystemsRx.Tests.Models;
using SystemsRx.Tests.SystemsRx.Handlers.Helpers;
using NSubstitute;
using Xunit;

namespace SystemsRx.Tests.SystemsRx.Handlers
{
    public class ReactToEventSystemHandlerTests
    {
        public interface MultipleOfSameInterface : IReactToEventSystem<int>, IReactToEventSystem<float>{}
        
        [Fact]
        public void should_correctly_handle_systems()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var reactToEventSystemHandler = new ReactToEventSystemHandler(mockEventSystem);
            
            var fakeMatchingSystem = Substitute.For<IReactToEventSystem<int>>();
            var fakeNonMatchingSystem1 = Substitute.For<ISystem>();
            var fakeNonMatchingSystem2 = Substitute.For<ISystem>();
            
            Assert.True(reactToEventSystemHandler.CanHandleSystem(fakeMatchingSystem));
            Assert.False(reactToEventSystemHandler.CanHandleSystem(fakeNonMatchingSystem1));
            Assert.False(reactToEventSystemHandler.CanHandleSystem(fakeNonMatchingSystem2));
        }
        
        [Fact]
        public void should_destroy_and_dispose_system()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSystem = Substitute.For<IReactToEventSystem<int>>();
            var mockDisposable = Substitute.For<IDisposable>();
            
            var systemHandler = new ReactToEventSystemHandler(mockEventSystem);
            systemHandler._systemSubscriptions.Add(mockSystem, mockDisposable);
            systemHandler.DestroySystem(mockSystem);
            
            mockDisposable.Received(1).Dispose();
            Assert.Equal(0, systemHandler._systemSubscriptions.Count);
        }

        [Fact]
        public void should_get_event_types_from_system()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSystem = Substitute.For<IReactToEventSystem<int>>();
            
            var systemHandler = new ReactToEventSystemHandler(mockEventSystem);
            var actualTypes = systemHandler.GetEventTypesFromSystem(mockSystem).ToArray();
            
            Assert.Equal(1, actualTypes.Length);
            Assert.Equal(typeof(int), actualTypes[0]);
        }
        
        [Fact]
        public void should_get_multiple_event_types_from_system()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSystem = Substitute.For<MultipleOfSameInterface>();
            
            var systemHandler = new ReactToEventSystemHandler(mockEventSystem);
            var actualTypes = systemHandler.GetEventTypesFromSystem(mockSystem).ToArray();
            
            Assert.Equal(2, actualTypes.Length);
            Assert.Contains(typeof(int), actualTypes);
            Assert.Contains(typeof(float), actualTypes);
        }

        [Fact]
        public void should_process_events_with_multiple_interfaces()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var dummyObjectSubject = new Subject<ComplexObject>();
            var dummyIntSubject = new Subject<int>();
            mockEventSystem.Receive<ComplexObject>().Returns(dummyObjectSubject);
            mockEventSystem.Receive<int>().Returns(dummyIntSubject);
            
            var mockSystem = Substitute.For<ITestMultiReactToEvent>();
            var dummyObject = new ComplexObject(10, "Bob");
            var dummyInt = 100;
            
            var systemHandler = new ReactToEventSystemHandler(mockEventSystem);
            systemHandler.SetupSystem(mockSystem);
            dummyObjectSubject.OnNext(dummyObject);
            dummyIntSubject.OnNext(dummyInt);
            
            mockSystem.Received(1).Process(Arg.Is(dummyObject));
            mockSystem.Received(1).Process(Arg.Is(dummyInt));
        }
    }
}