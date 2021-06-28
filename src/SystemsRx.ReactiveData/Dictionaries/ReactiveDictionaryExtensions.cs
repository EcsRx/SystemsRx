using System.Collections.Generic;

namespace SystemsRx.ReactiveData.Dictionaries
{
    public static partial class ReactiveDictionaryExtensions
    {
        public static ReactiveDictionary<TKey, TValue> ToReactiveDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new ReactiveDictionary<TKey, TValue>(dictionary);
        }
    }
}