using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using SystemsRx.Tests.Ninject.TestCode;

namespace SystemsRx.Tests.Ninject.Modules
{
    public class TestModule : IDependencyModule
    {
        public void Setup(IDependencyRegistry registry)
        {
            registry.Bind<SomeTestClass>();
            registry.Bind<SomeTestMethodClass>(x => x.ToMethod(y => new SomeTestMethodClass()));
            registry.Bind<SomeDerivedTestClass>();
            registry.Bind<SomeBaseTestClass>(x => x.ToBoundType<SomeDerivedTestClass>());
            registry.Bind<ITestInterface, TestClass1>(x => x.WithName("Test1"));
            registry.Bind<ITestInterface, TestClass2>(x => x.WithName("Test2"));
        }
    }
}