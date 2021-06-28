using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using SystemsRx.ReactiveData;
using SystemsRx.ReactiveData.Collections;
using SystemsRx.ReactiveData.Extensions;
using Xunit;

namespace SystemsRx.Tests.Sanity
{
    public class SanityTests
    {
        [Fact]
        public void should_notify_and_update_values_with_reactive_property()
        {
            var reactiveProperty = new ReactiveProperty<int>(10);
            Assert.Equal(10, reactiveProperty.Value);

            var timesEntered = 0;
            var sub = reactiveProperty.Subscribe(x =>
            {
                if(timesEntered == 0) { Assert.Equal(10, x); }
                if(timesEntered == 1) { Assert.Equal(7, x); }
                timesEntered++;
            });
            reactiveProperty.Value = 7;
            Assert.Equal(7, reactiveProperty.Value);
            Assert.Equal(2, timesEntered);
            
            sub.Dispose();
        }
        
        [Fact]
        public void should_notify_and_update_values_with_reactive_collection()
        {
            var initial = new List<int> {1, 2, 3};
            var reactiveCollection = new ReactiveCollection<int>(initial);
            Assert.Equal(3, reactiveCollection.Count);
            Assert.Equal(initial, reactiveCollection);
            
            var timesEntered = 0;
            var sub = reactiveCollection.ObserveAdd().Subscribe(x =>
            {
                if(timesEntered == 0) { Assert.Equal(6, x.Value); }
                if(timesEntered == 1) { Assert.Equal(7, x.Value); }
                timesEntered++;
            });
            
            reactiveCollection.Add(6);
            reactiveCollection.Add(7);
            initial.Add(6);
            initial.Add(7);
            Assert.Equal(initial, reactiveCollection);
            Assert.Equal(2, timesEntered);
            
            sub.Dispose();
        }
        
        [Fact]
        public void should_notify_and_update_values_with_reactive_property_from_observable()
        {
            var reactiveProperty = Observable.Return(10).ToReactiveProperty();
            Assert.Equal(10, reactiveProperty.Value);

            var timesEntered = 0;
            var sub = reactiveProperty.Subscribe(x =>
            {
                if(timesEntered == 0) { Assert.Equal(10, x); }
                if(timesEntered == 1) { Assert.Equal(7, x); }
                timesEntered++;
            });
            reactiveProperty.Value = 7;
            Assert.Equal(7, reactiveProperty.Value);
            
            sub.Dispose();
        }
    }
}