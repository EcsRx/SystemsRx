using System;
using System.Collections.Generic;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Systems;

namespace SystemsRx.Infrastructure.Plugins
{
    public interface ISystemsRxPlugin
    {
        string Name { get; }
        Version Version { get; }

        void SetupDependencies(IDependencyRegistry registry);
        IEnumerable<ISystem> GetSystemsForRegistration(IDependencyResolver resolver);
    }
}