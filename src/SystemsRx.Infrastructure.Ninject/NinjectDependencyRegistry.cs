using System;
using System.Linq;
using SystemsRx.Extensions;
using SystemsRx.Infrastructure.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace SystemsRx.Infrastructure.Ninject
{
    /// <summary>
    /// This is a ninject implementation for the dependency registry.
    /// </summary>
    public class NinjectDependencyRegistry : IDependencyRegistry
    {
        private readonly IKernel _kernel;
            
        public NinjectDependencyRegistry(IKernel kernel = null)
        {
            _kernel = kernel ?? new StandardKernel();
            Resolver = new NinjectDependencyResolver(_kernel);
        }

        public object NativeRegistry => _kernel;
        public IDependencyResolver Resolver { get; }

        public void Bind(Type fromType, Type toType, BindingConfiguration configuration = null)
        {
            var bindingSetup = _kernel.Bind(fromType);
            
            if (configuration == null)
            {
                bindingSetup.To(toType).InSingletonScope();
                return;
            }

            IBindingWhenInNamedWithOrOnSyntax<object> binding;

            if (configuration.ToInstance != null)
            { binding = bindingSetup.ToConstant(configuration.ToInstance); }
            else if (configuration.ToMethod != null)
            { binding = bindingSetup.ToMethod(x => configuration.ToMethod(Resolver)); }
            else
            {
                binding = bindingSetup.To(toType);
                
                foreach (var constructorArg in configuration.WithNamedConstructorArgs)
                { binding.WithConstructorArgument(constructorArg.Key, constructorArg.Value); }
            
                foreach (var constructorArg in configuration.WithTypedConstructorArgs)
                { binding.WithConstructorArgument(constructorArg.Key, constructorArg.Value); }
            }
            
            if(configuration.AsSingleton)
            { binding.InSingletonScope(); }
            
            if(!string.IsNullOrEmpty(configuration.WithName))
            { binding.Named(configuration.WithName); }

            if (configuration.OnActivation != null)
            { binding.OnActivation(instance => configuration.OnActivation(Resolver, instance)); }

            if (configuration.WhenInjectedInto.Count != 0)
            { configuration.WhenInjectedInto.ForEachRun(x => binding.WhenInjectedInto(x)); }
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public bool HasBinding(Type type, string name = null)
        {
            var applicableBindings = _kernel.GetBindings(type);
             
            if(string.IsNullOrEmpty(name))
            { return applicableBindings.Any(); }
            
            return applicableBindings.Any(x => x.Metadata.Name == name);
        }

        public void Unbind(Type type)
        { _kernel.Unbind(type); }

        public void LoadModule(IDependencyModule module)
        { module.Setup(this); }

        public IDependencyResolver BuildResolver()
        { return Resolver; }

        public void Dispose()
        { _kernel?.Dispose(); }
    }
}