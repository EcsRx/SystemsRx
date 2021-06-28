using System.Collections.Generic;
using SystemsRx.Systems;

namespace SystemsRx.Executor
{
    public interface ISystemExecutor
    {
        IEnumerable<ISystem> Systems { get; }

        bool HasSystem(ISystem system);
        void RemoveSystem(ISystem system);
        void AddSystem(ISystem system);
    }
}