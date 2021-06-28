using System.Collections.Generic;

namespace SystemsRx.ReactiveData.Dictionaries
{
    public interface IReactiveDictionary<TKey, TValue> : IReadOnlyReactiveDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
    }
}