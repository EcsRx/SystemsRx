using Autofac;
using SystemsRx.Infrastructure.Dependencies;

namespace SystemsRx.Infrastructure.Autofac.Extensions
{
    public static class DependencyExtensions
    {
        public static ContainerBuilder GetContainerBuilder(this IDependencyRegistry registry)
        { return registry.NativeRegistry as ContainerBuilder; }
        
        public static IContainer GetContainer(this IDependencyResolver resolver)
        { return resolver.NativeResolver as IContainer; }
    }
}