using System;
using Microsoft.Extensions.DependencyInjection;
using SystemsRx.Infrastructure.Dependencies;

namespace SystemsRx.Infrastructure.MicrosoftDependencyInjection.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection GetServiceCollection(this IDependencyRegistry registry)
        { return registry.NativeRegistry as IServiceCollection; }
        
        public static IServiceProvider GetServiceProvider(this IDependencyResolver resolver)
        { return resolver.NativeResolver as IServiceProvider; }
    }
}