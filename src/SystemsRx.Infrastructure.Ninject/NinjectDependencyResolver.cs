using System;
using System.Collections;
using Ninject;
using SystemsRx.Infrastructure.Dependencies;

namespace SystemsRx.Infrastructure.Ninject
{
    /// <summary>
    /// This is a ninject implementation for the dependency resolver
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        public object NativeResolver => _kernel;

        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel = null)
        {
            _kernel = kernel ?? new StandardKernel();
        }
        
        public IEnumerable ResolveAll(Type type)
        { return _kernel.GetAll(type); }
        
        public object Resolve(Type type, string name = null)
        { return string.IsNullOrEmpty(name) ? _kernel.Get(type) : _kernel.Get(type, name); }

        public void Dispose()
        {
            // Registry disposes Kernel
        }
    }
}