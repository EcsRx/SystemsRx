using System;

namespace SystemsRx.Systems.Conventional
{
    public interface IReactiveSystem<T> : ISystem
    {
        /// <summary>
        /// Returns and observable indicating when the system should execute
        /// </summary>
        /// <returns>Observable indicating when the system should execute</returns>
        IObservable<T> ReactTo();
        
        /// <summary>
        /// Thhe method to execute on triggering
        /// </summary>
        void Execute(T data);
    }
}