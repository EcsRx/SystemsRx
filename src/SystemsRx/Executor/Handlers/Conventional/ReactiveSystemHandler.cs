using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemsRx.Attributes;
using SystemsRx.Events;
using SystemsRx.Extensions;
using SystemsRx.MicroRx.Disposables;
using SystemsRx.MicroRx.Extensions;
using SystemsRx.Systems;
using SystemsRx.Systems.Conventional;

namespace SystemsRx.Executor.Handlers.Conventional
{
    [Priority(6)]
    public class ReactiveSystemHandler : IConventionalSystemHandler
    {
        private readonly MethodInfo _setupSystemGenericMethodInfo;
        public readonly IDictionary<ISystem, IDisposable> _systemSubscriptions;

        public ReactiveSystemHandler()
        {
            _systemSubscriptions = new Dictionary<ISystem, IDisposable>();
            _setupSystemGenericMethodInfo = typeof(ReactiveSystemHandler).GetMethod(nameof(SetupSystemGeneric));
        }

        public bool CanHandleSystem(ISystem system)
        { return system.IsReactiveSystem(); }

        public Type[] GetMatchingInterfaces(ISystem system)
        { return system.GetGenericInterfacesFor(typeof(IReactiveSystem<>)).ToArray(); }
        
        public void SetupSystem(ISystem system)
        {
            var matchingInterfaces = GetMatchingInterfaces(system);
            var disposables = new List<IDisposable>();
            foreach (var matchingInterface in matchingInterfaces)
            {
                var dataType = matchingInterface.GetGenericArguments()[0];
                var disposable = (IDisposable)_setupSystemGenericMethodInfo.MakeGenericMethod(dataType).Invoke(this, new object[] { system });
                disposables.Add(disposable);
            }
            _systemSubscriptions.Add(system, new CompositeDisposable(disposables));
        }

        public IDisposable SetupSystemGeneric<T>(IReactiveSystem<T> system)
        { return system.ReactTo().Subscribe(system.Execute); }
        
        public void DestroySystem(ISystem system)
        { _systemSubscriptions.RemoveAndDispose(system); }

        public void Dispose()
        {
            _systemSubscriptions.Values.DisposeAll();
            _systemSubscriptions.Clear();
        }
    }
}
