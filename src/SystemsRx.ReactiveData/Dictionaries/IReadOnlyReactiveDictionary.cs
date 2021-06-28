﻿using System;
using System.Collections.Generic;
using SystemsRx.MicroRx;

namespace SystemsRx.ReactiveData.Dictionaries
{
    public interface IReadOnlyReactiveDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDisposable
    {
        int Count { get; }
        TValue this[TKey index] { get; }
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);

        IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd();
        IObservable<int> ObserveCountChanged();
        IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove();
        IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace();
        IObservable<Unit> ObserveReset();
    }
}