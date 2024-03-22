﻿using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Exceptions;

namespace SystemsRx.Infrastructure.MicrosoftDependencyInjection
{
    /// <summary>
    /// This is a ninject implementation for the dependency registry.
    /// </summary>
    public class MicrosoftDependencyRegistry : IDependencyRegistry
    {
        private readonly IServiceCollection _serviceCollection;
        private IDependencyResolver _resolver;
        public object NativeRegistry => _serviceCollection;

        public MicrosoftDependencyRegistry(IServiceCollection serviceCollection = null)
        {
            _serviceCollection = serviceCollection ?? new ServiceCollection();
        }

        public void Bind(Type fromType, Type toType, BindingConfiguration configuration = null)
        {
            if (configuration == null)
            {
                _serviceCollection.AddSingleton(fromType, toType);
                return;
            }

            var serviceLifetime = configuration.AsSingleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient;
            ServiceDescriptor serviceDescriptor;
            
            if (configuration.ToInstance != null)
            { serviceDescriptor = new ServiceDescriptor(fromType, configuration.WithName, configuration.ToInstance); }
            else if (configuration.ToMethod != null)
            {
                serviceDescriptor = new ServiceDescriptor(fromType, configuration.WithName, (x, _) =>
                {
                    var resolution = configuration.ToInstance ?? configuration.ToMethod(_resolver);
                    configuration.OnActivation?.Invoke(_resolver, resolution);
                    return resolution;
                }, serviceLifetime);
            }
            else
            {
                throw new BindingException(
                    "Microsoft Dependency Injection does not support constructor parameters, it is recommended you manually build it via a method");
            }
            
            _serviceCollection.Add(serviceDescriptor);
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public bool HasBinding(Type type, string name = null)
        {
            var applicableBindings = _serviceCollection.Where(x => x.ServiceType == type);
             
            if(string.IsNullOrEmpty(name))
            { return applicableBindings.Any(); }
            
            return applicableBindings.Any(x => x.ServiceKey?.Equals(name) ?? false);
        }

        public void Unbind(Type type)
        {
            var applicableBindings = _serviceCollection.Where(x => x.ServiceType == type).ToArray();
            foreach(var applicableBinding in applicableBindings)
            { _serviceCollection.Remove(applicableBinding); }
        }

        public void LoadModule(IDependencyModule module)
        { module.Setup(this); }

        public IDependencyResolver BuildResolver()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            _resolver = new MicrosoftDependencyResolver(serviceProvider);
            return _resolver;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}