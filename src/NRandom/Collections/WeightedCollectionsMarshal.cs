using System.Runtime.InteropServices;

namespace NRandom.Collections;

/// <summary>
/// An unsafe class that provides a set of methods to access the underlying data representations of weighted collections.
/// </summary>
public static class WeightedCollectionsMarshal
{
    /// <summary>
    /// Get a Span view over a WeightedList's data.
    /// </summary>
    public static Span<WeightedValue<T>> AsSpan<T>(WeightedList<T> list)
    {
        return CollectionsMarshal.AsSpan(list.GetListInternal());
    }

    /// <summary>
    /// Sets the count and totalWeight of the WeightedList
    /// </summary>
    public static void SetCountAndTotalWeight<T>(WeightedList<T> list, int count, double totalWeight)
    {
#if NET8_0_OR_GREATER
        CollectionsMarshal.SetCount(list.GetListInternal(), count);
#else
        CollectionsMarshal.UnsafeSetCount(list.GetListInternal(), count);
#endif

        list.SetTotalWeightInternal(totalWeight);
    }
}