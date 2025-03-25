using System.Runtime.InteropServices;
using NRandom.Collections;

namespace NRandom.Linq;

/// <summary>
/// Provides additional LINQ methods that utilize random numbers.
/// </summary>
public static class RandomEnumerable
{
    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<int> Repeat(int min, int max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<int> Repeat(int min, int max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextInt(min, max);
        }
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<uint> Repeat(uint min, uint max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<uint> Repeat(uint min, uint max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextUInt(min, max);
        }
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<long> Repeat(long min, long max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<long> Repeat(long min, long max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextLong(min, max);
        }
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<ulong> Repeat(ulong min, ulong max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<ulong> Repeat(ulong min, ulong max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextULong(min, max);
        }
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<float> Repeat(float min, float max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<float> Repeat(float min, float max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextFloat(min, max);
        }
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<double> Repeat(double min, double max, int length)
    {
        return Repeat(min, max, length, RandomEx.Shared);
    }

    /// <summary>
    /// Creates a repeating sequence of random values ​​in the range [min, max).
    /// </summary>
    public static IEnumerable<double> Repeat(double min, double max, int length, IRandom random)
    {
        for (int i = 0; i < length; i++)
        {
            yield return random.NextDouble(min, max);
        }
    }

    /// <summary>
    /// Returns a random element of a sequence.
    /// </summary>
    public static T RandomElement<T>(this IEnumerable<T> source)
    {
        return RandomElement(source, RandomEx.Shared);
    }

    /// <summary>
    /// Returns a random element of a sequence.
    /// </summary>
    public static T RandomElement<T>(this IEnumerable<T> source, IRandom random)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source is T[] array)
        {
            return random.GetItem(array);
        }
        if (source is List<T> list)
        {
            return random.GetItem<T>(CollectionsMarshal.AsSpan(list));
        }
        if (source is IReadOnlyList<T> readOnlyList)
        {
            if (readOnlyList.Count == 0) throw new ArgumentException("The collection is empty.");
            return readOnlyList[random.NextInt(0, readOnlyList.Count)];
        }

        var count = source.Count();
        if (count == 0) throw new ArgumentException("The collection is empty.");

        return source.ElementAt(random.NextInt(0, count));
    }

    /// <summary>
    /// Returns a shuffled sequence.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return Shuffle(source, RandomEx.Shared);
    }

    /// <summary>
    /// Returns a shuffled sequence.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, IRandom random)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var buffer = source.ToArray();

        for (int i = 0; i < buffer.Length; i++)
        {
            int j = random.NextInt(i, buffer.Length);
            yield return buffer[j];
            buffer[j] = buffer[i];
        }
    }

    /// <summary>
    /// Creates a WeightedList from an IEnumerable according to a specified weight selector function.
    /// </summary>
    public static WeightedList<T> ToWeightedList<T>(this IEnumerable<T> source, Func<T, double> weightSelector)
    {
        ThrowHelper.ThrowIfNull(source);
        ThrowHelper.ThrowIfNull(weightSelector);

        var list = new WeightedList<T>();

        foreach (var item in source)
        {
            list.Add(item, weightSelector(item));
        }

        return list;
    }
}