using System.Numerics;

namespace NRandom;

public static partial class RandomEx
{
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
}