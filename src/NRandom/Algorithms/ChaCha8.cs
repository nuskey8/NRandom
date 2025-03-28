using System.Numerics;
using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

// Original Implementation: https://cr.yp.to/streamciphers/timings/estreambench/submissions/salsa20/chacha8/ref/chacha.c

/// <summary>
/// Implementation of ChaCha8
/// </summary>
public unsafe struct ChaCha8
{
    fixed uint state[16];

    public void Init(ReadOnlySpan<uint> key, uint count, ReadOnlySpan<uint> nonce)
    {
        ExpectLength(nameof(key), key.Length, 8);
        ExpectLength(nameof(key), nonce.Length, 2);

        // Constants
        state[0] = 0x61707865;  // "expa"
        state[1] = 0x3320646e;  // "nd 3"
        state[2] = 0x79622d32;  // "2-by"
        state[3] = 0x6b206574;  // "te k"

        // Key
        state[4] = key[0];
        state[5] = key[1];
        state[6] = key[2];
        state[7] = key[3];
        state[8] = key[4];
        state[9] = key[5];
        state[10] = key[6];
        state[11] = key[7];

        // Counter
        state[12] = count;
        state[13] = 0;

        // Nonce
        state[14] = nonce[0];
        state[15] = nonce[1];
    }

    public unsafe void Next(Span<uint> buffer)
    {
        ExpectLength(nameof(buffer), buffer.Length, 16);

        fixed (uint* ptr = state)
        {
            Next(new Span<uint>(ptr, 16), buffer);
        }
    }

    public static void Next(Span<uint> state, Span<uint> buffer)
    {
        ExpectLength(nameof(state), state.Length, 16);
        ExpectLength(nameof(buffer), buffer.Length, 16);

        state.CopyTo(buffer[..16]);

        for (int i = 0; i < 8; i += 2)
        {
            QuarterRound(buffer, 0, 4, 8, 12);
            QuarterRound(buffer, 1, 5, 9, 13);
            QuarterRound(buffer, 2, 6, 10, 14);
            QuarterRound(buffer, 3, 7, 11, 15);

            QuarterRound(buffer, 0, 5, 10, 15);
            QuarterRound(buffer, 1, 6, 11, 12);
            QuarterRound(buffer, 2, 7, 8, 13);
            QuarterRound(buffer, 3, 4, 9, 14);
        }

        state[12] = unchecked(state[12] + 1);
        if (state[12] <= 0)
        {
            state[13] = unchecked(state[13] + 1);
        }

        for (int i = 0; i < 16; i++)
        {
            buffer[i] += state[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void QuarterRound(Span<uint> x, int a, int b, int c, int d)
    {
        x[a] += x[b]; x[d] ^= x[a]; x[d] = BitOperations.RotateLeft(x[d], 16);
        x[c] += x[d]; x[b] ^= x[c]; x[b] = BitOperations.RotateLeft(x[b], 12);
        x[a] += x[b]; x[d] ^= x[a]; x[d] = BitOperations.RotateLeft(x[d], 8);
        x[c] += x[d]; x[b] ^= x[c]; x[b] = BitOperations.RotateLeft(x[b], 7);
    }

    static void ExpectLength(string paramName, int actual, int expected)
    {
        if (actual < expected) throw new ArgumentException($"'{paramName}' must have length {expected}", paramName);
    }
}