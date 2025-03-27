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

    public void Init(uint seed)
    {
        state[0] = 0x61707865;  // "expa"
        state[1] = 0x3320646e;  // "nd 3"
        state[2] = 0x79622d32;  // "2-by"
        state[3] = 0x6b206574;  // "te k"

        state[4] = seed;
        state[5] = seed ^ 0xDEADBEEF;
        state[6] = seed ^ 0xCAFEBABE;
        state[7] = seed ^ 0xBAADF00D;

        state[8] = 0;  // Counter
        state[9] = 0;
        state[10] = 0;
        state[11] = 0;

        state[12] = 0x12345678; // Nonce
        state[13] = 0x9ABCDEF0;
        state[14] = 0xDEADBEEF;
        state[15] = 0xCAFEBABE;
    }

    public unsafe void Next(Span<uint> buffer)
    {
        ThrowIfLengthIsNot16(nameof(buffer), buffer.Length);

        fixed (uint* ptr = state)
        {
            Next(new ReadOnlySpan<uint>(ptr, 16), buffer);
        }
    }

    public static void Next(ReadOnlySpan<uint> state, Span<uint> buffer)
    {
        ThrowIfLengthIsNot16(nameof(state), state.Length);
        ThrowIfLengthIsNot16(nameof(buffer), buffer.Length);

        state.CopyTo(buffer);

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

    static void ThrowIfLengthIsNot16(string paramName, int length)
    {
        if (length != 16) throw new ArgumentException($"'{paramName}' must have length 16", paramName);
    }
}