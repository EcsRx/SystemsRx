using System;
using System.Collections.Generic;
using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Plugins;
using SystemsRx.Systems;

namespace SystemsRx.Plugins.Computeds
{
    public class ComputedsPlugin : ISystemsRxPlugin
    {
        public string Name => "SystemsRx Computeds";
        public Version Version { get; } = new Version("1.0.0");
        
        public void SetupDependencies(IDependencyContainer container)
        {
            // Nothing needs registering
        }
        
        public IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container) => Array.Empty<ISystem>();
    }
}