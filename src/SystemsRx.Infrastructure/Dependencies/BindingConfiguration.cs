using System;
using System.Collections.Generic;

namespace SystemsRx.Infrastructure.Dependencies
{
    public class BindingConfiguration
    {
        public bool AsSingleton { get; set; } = true;
        public string WithName { get; set; }
        public object ToInstance { get; set; }
        public Func<IDependencyResolver, object> ToMethod { get; set; }
    }
}