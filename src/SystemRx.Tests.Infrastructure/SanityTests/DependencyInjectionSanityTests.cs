﻿using System.Collections.Generic;
using System.Linq;
using SystemsRx.Executor.Handlers;
using SystemsRx.Infrastructure.Autofac;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.DryIoc;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Infrastructure.MicrosoftDependencyInjection;
using SystemsRx.Infrastructure.Modules;
using SystemsRx.Infrastructure.Ninject;
using SystemsRx.Scheduling;
using SystemsRx.Tests.Ninject.Modules;
using SystemsRx.Tests.Ninject.TestCode;
using Xunit;

namespace SystemsRx.Tests.Ninject.SanityTests
{
    public class DependencyInjectionSanityTests
    {
        public static IEnumerable<object[]> Registries => new List<object[]>()
        {
            new object[]{new NinjectDependencyRegistry()},
            new object[]{new MicrosoftDependencyRegistry()},
            new object[]{new AutofacDependencyRegistry()},
            new object[]{new DryIocDependencyRegistry()},
        };
        
        [Theory]
        [MemberData(nameof(Registries))]
        public void should_handle_test_module_bindings_correctly(IDependencyRegistry registryType)
        {
            registryType.LoadModule<TestModule>();
            
            var resolver = registryType.BuildResolver();

            var someTestClass = resolver.Resolve<SomeTestClass>();
            Assert.NotNull(someTestClass);

            var someTestMethodClass = resolver.Resolve<SomeTestMethodClass>();
            Assert.NotNull(someTestMethodClass);

            var someBaseTestClass = resolver.Resolve<SomeBaseTestClass>();
            Assert.NotNull(someBaseTestClass);
            Assert.IsAssignableFrom<SomeDerivedTestClass>(someBaseTestClass);
            
            var testNamed1 = resolver.Resolve<ITestInterface>("Test1");
            Assert.NotNull(testNamed1);
            Assert.IsAssignableFrom<ITestInterface1>(testNamed1);
            Assert.IsAssignableFrom<TestClass1>(testNamed1);
            
            var testNamed2 = resolver.Resolve<ITestInterface>("Test2");
            Assert.NotNull(testNamed2);
            Assert.IsAssignableFrom<ITestInterface2>(testNamed2);
            Assert.IsAssignableFrom<TestClass2>(testNamed2);
            
            var testInterfaces = resolver.ResolveAll<ITestInterface>();
            Assert.NotNull(testInterfaces);
            
            var testInterfaceEnumerated = testInterfaces.ToArray();
            Assert.NotEmpty(testInterfaceEnumerated);
            Assert.Equal(2, testInterfaceEnumerated.Length);
            Assert.True(testInterfaceEnumerated.All(x => x != null));
        }
        
        [Theory]
        [MemberData(nameof(Registries))]
        public void should_bind_and_resolve_framework_module(IDependencyRegistry registryType)
        {
            registryType.LoadModule<FrameworkModule>();

            var resolver = registryType.BuildResolver();
            var systems = resolver.ResolveAll<IConventionalSystemHandler>();
            Assert.NotNull(systems);
            Assert.NotEmpty(systems);
            Assert.Equal(3, systems.Count());
                
            var timeTracker = resolver.Resolve<ITimeTracker>();
            Assert.NotNull(timeTracker);
        }

        [Theory]
        [MemberData(nameof(Registries))]
        public void should_always_return_singletons_by_default(IDependencyRegistry registryType)
        {
            registryType.Bind<ITestInterface, TestClass1>();
            var resolver = registryType.BuildResolver();

            var firstResolution = resolver.Resolve<ITestInterface>();
            var secondResolution = resolver.Resolve<ITestInterface>();
            
            Assert.Same(firstResolution, secondResolution);
        }
    }
}