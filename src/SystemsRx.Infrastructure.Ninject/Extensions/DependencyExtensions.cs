using Ninject;
using SystemsRx.Infrastructure.Dependencies;

namespace SystemsRx.Infrastructure.Ninject.Extensions
{
    public static class DependencyExtensions
    {
        public static IKernel GetKernel(this IDependencyRegistry registry)
        { return registry.NativeRegistry as IKernel; }
        
        public static IKernel GetKernel(this IDependencyResolver resolver)
        { return resolver.NativeResolver as IKernel; }
    }
}