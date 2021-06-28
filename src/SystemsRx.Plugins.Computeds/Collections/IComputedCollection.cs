using System;
using System.Collections.Generic;
using SystemsRx.Computeds;
using SystemsRx.Plugins.Computeds.Collections.Events;

namespace SystemsRx.Plugins.Computeds.Collections
{
    /// <summary>
    /// Represents a computed collection of elements
    /// </summary>
    /// <typeparam name="T">The data to contain</typeparam>
    public interface IComputedCollection<T> : IComputed<IEnumerable<T>>, IEnumerable<T>
    {
        /// <summary>
        /// Get an element by its index
        /// </summary>
        /// <remarks>
        /// In some implementations this may be its id not a sequential index
        /// </remarks>
        /// <param name="index">index/id of the element</param>
        T this[int index] {get;}

        /// <summary>
        /// How many elements are within the collection
        /// </summary>
        int Count { get; }
    }
}