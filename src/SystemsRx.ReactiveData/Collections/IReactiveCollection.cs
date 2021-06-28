using System.Collections.Generic;

namespace SystemsRx.ReactiveData.Collections
{
    public interface IReactiveCollection<T> : IList<T>, IReadOnlyReactiveCollection<T>
    {
        new int Count { get; }
        new T this[int index] { get; set; }
        void Move(int oldIndex, int newIndex);
    }
}