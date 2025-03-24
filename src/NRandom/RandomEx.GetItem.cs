using System.Numerics;

namespace NRandom;

public static partial class RandomEx
{
    /// <summary>
    /// Returns a random element from the provided choices.
    /// </summary>
    public static T GetItem<T>(this IRandom random, T[] choices)
    {
        ThrowHelper.ThrowIfNull(choices);
        return GetItem(random, new ReadOnlySpan<T>(choices));
    }

    /// <summary>
    /// Returns a random element from the provided choices.
    /// </summary>
    public static T GetItem<T>(this IRandom random, ReadOnlySpan<T> choices)
    {
        ThrowHelper.ThrowIfEmpty(choices);
        return choices[random.NextInt(choices.Length)];
    }

    /// <summary>
    /// Returns a random element based on the weights.
    /// </summary>
    public static T GetItem<T>(this IRandom random, ReadOnlySpan<T> choices, ReadOnlySpan<double> weights)
    {
        ThrowHelper.ThrowIfEmpty(choices);

        if (choices.Length != weights.Length)
        {
            throw new ArgumentException("The number of elements in weights does not match choices.");
        }

        var total = 0.0;
        foreach (var w in weights)
        {
            total += w;
        }

        var r = random.NextDouble() * total;
        var current = 0.0;

        for (int i = 0; i < weights.Length; i++)
        {
            current += weights[i];
            if (r <= current)
            {
                return choices[i];
            }
        }

        return default!;
    }

    /// <summary>
    /// Creates an array populated with items chosen at random from the provided set of choices.
    /// </summary>
    public static T[] GetItems<T>(this IRandom random, ReadOnlySpan<T> choices, int length)
    {
        ThrowHelper.ThrowIfLengthIsNegative(length);

        var destination = new T[length];
        GetItems(random, choices, destination.AsSpan());

        return destination;
    }

    /// <summary>
    /// Creates an array populated with items chosen at random from the provided set of choices.
    /// </summary>
    public static T[] GetItems<T>(this IRandom random, ReadOnlySpan<T> choices, ReadOnlySpan<double> weights, int length)
    {
        ThrowHelper.ThrowIfLengthIsNegative(length);

        var destination = new T[length];
        GetItems(random, choices, weights, destination.AsSpan());

        return destination;
    }

    /// <summary>
    /// Fills the elements of a specified span with items chosen at random from the provided set of choices.
    /// </summary>
    public static void GetItems<T>(this IRandom random, ReadOnlySpan<T> choices, Span<T> destination)
    {
        ThrowHelper.ThrowIfEmpty(choices);

        if (BitOperations.IsPow2(choices.Length) && choices.Length <= 256)
        {
            Span<byte> randomBytes = stackalloc byte[512];

            while (!destination.IsEmpty)
            {
                if (destination.Length < randomBytes.Length)
                {
                    randomBytes = randomBytes[..destination.Length];
                }

                random.NextBytes(randomBytes);

                int mask = choices.Length - 1;
                for (int i = 0; i < randomBytes.Length; i++)
                {
                    destination[i] = choices[randomBytes[i] & mask];
                }

                destination = destination[randomBytes.Length..];
            }

            return;
        }

        for (int i = 0; i < destination.Length; i++)
        {
            destination[i] = choices[random.NextInt(choices.Length)];
        }
    }

    /// <summary>
    /// Fills the elements of a specified span with items chosen at random from the provided set of choices.
    /// </summary>
    public static void GetItems<T>(this IRandom random, ReadOnlySpan<T> choices, ReadOnlySpan<double> weights, Span<T> destination)
    {
        ThrowHelper.ThrowIfEmpty(choices);

        for (int i = 0; i < destination.Length; i++)
        {
            destination[i] = GetItem(random, choices, weights);
        }
    }
}