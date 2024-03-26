using DryIoc;
using SystemsRx.Infrastructure.Dependencies;

namespace SystemsRx.Infrastructure.DryIoc.Extensions
{
    public static class DependencyExtensions
    {
        public static Container GetContainer(this IDependencyRegistry registry)
        { return registry.NativeRegistry as Container; }
        
        public static Container GetContainer(this IDependencyResolver resolver)
        { return resolver.NativeResolver as Container; }
    }
}