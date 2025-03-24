using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NRandom.Algorithms;

namespace NRandom;

public static partial class RandomEx
{
    /// <summary>
    /// Returns a random bool value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(this IRandom random)
    {
        return (random.NextUInt() & 1) == 1;
    }

    /// <summary>
    /// Returns a random uint value in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint NextUInt(this IRandom random, uint max)
    {
        return (uint)((random.NextUInt() * (ulong)max) >> 32);
    }

    /// <summary>
    /// Returns a random uint value in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint NextUInt(this IRandom random, uint min, uint max)
    {
        ThrowHelper.CheckMinMax(min, max);
        uint range = max - min;
        return (uint)(random.NextUInt() * (ulong)range >> 32) + min;
    }

    /// <summary>
    /// Returns a random int value in [int.MinValue, int.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(this IRandom random)
    {
        return (int)random.NextUInt() ^ -2147483648;
    }

    /// <summary>
    /// Returns a random int value in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(this IRandom random, int max)
    {
        ThrowHelper.CheckMax(max);
        return (int)((random.NextUInt() * (ulong)max) >> 32);
    }

    /// <summary>
    /// Returns a random int value in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(this IRandom random, int min, int max)
    {
        ThrowHelper.CheckMinMax(min, max);
        var range = (uint)(max - min);
        return (int)(random.NextUInt() * (ulong)range >> 32) + min;
    }

    /// <summary>
    /// Returns a random ulong value in [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong NextULong(this IRandom random, ulong max)
    {
        if (max <= uint.MaxValue)
        {
            return random.NextUInt((uint)max);
        }

        if (max > 1)
        {
            int bits = Log2Ceiling(max);
            while (true)
            {
                ulong result = random.NextULong() >> (sizeof(ulong) * 8 - bits);
                if (result < max)
                {
                    return result;
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// Returns a random ulong value in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong NextULong(this IRandom random, ulong min, ulong max)
    {
        ThrowHelper.CheckMinMax(min, max);
        return NextULong(random, max - min) + min;
    }

    /// <summary>
    /// Returns a random long value in [long.MinValue, long.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long NextLong(this IRandom random)
    {
        return (long)random.NextULong();
    }

    /// <summary>
    /// Returns a random long value in [0, max).
    /// </summary>
    public static long NextLong(this IRandom random, long max)
    {
        ThrowHelper.CheckMax(max);

        if (max <= int.MaxValue)
        {
            return (int)((random.NextUInt() * (ulong)max) >> 32);
        }

        if (max > 1)
        {
            return (long)random.NextULong();
        }

        return 0;
    }

    /// <summary>
    /// Returns a random long value in [min, max).
    /// </summary>
    public static long NextLong(this IRandom random, long min, long max)
    {
        return NextLong(random, max - min) + min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int Log2Ceiling(ulong value)
    {
        int result = BitOperations.Log2(value);
        if (BitOperations.PopCount(value) != 1)
        {
            result++;
        }
        return result;
    }

    /// <summary>
    /// Returns a random float value in [0.0, 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(this IRandom random)
    {
        return (random.NextUInt() >> 8) * (1.0f / (1u << 24));
    }

    /// <summary>
    /// Returns a random float value in [0.0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(this IRandom random, float max)
    {
        ThrowHelper.CheckMax(max);
        return NextFloat(random) * max;
    }

    /// <summary>
    /// Returns a random float value in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(this IRandom random, float min, float max)
    {
        ThrowHelper.CheckMinMax(min, max);

        var r = NextFloat(random) * (max - min) + min;

        if (r >= max)
        {
#if !NET6_0_OR_GREATER
            r = MathEx.BitDecrement(max);
#else
            r = MathF.BitDecrement(max);
#endif
        }

        return r;
    }

    /// <summary>
    /// Returns a random double value in [0.0, 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NextDouble(this IRandom random)
    {
        return (random.NextULong() >> 11) * (1.0 / (1ul << 53));
    }

    /// <summary>
    /// Returns a random double value in [0.0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NextDouble(this IRandom random, double max)
    {
        ThrowHelper.CheckMax(max);
        return NextDouble(random) * max;
    }

    /// <summary>
    /// Returns a random double value in [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NextDouble(this IRandom random, double min, double max)
    {
        ThrowHelper.CheckMinMax(min, max);

        var r = NextDouble(random) * (max - min) + min;

        // correct for rounding
        if (r >= max)
        {
#if !NET6_0_OR_GREATER
            r = MathEx.BitDecrement(max);
#else
            r = Math.BitDecrement(max);
#endif
        }

        return r;
    }

    /// <summary>
    /// Returns a Gaussian distributed random double value with mean 0.0 and standard deviation 1.0.
    /// </summary>
    public static double NextDoubleGaussian(this IRandom random)
    {
        double v1, v2, s;
        do
        {
            v1 = 2 * NextDouble(random) - 1;
            v2 = 2 * NextDouble(random) - 1;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1 || s == 0);

        double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
        return v1 * multiplier;
    }

    /// <summary>
    /// Fills the buffer with random bytes [0..0x7f].
    /// </summary>
    public static unsafe void NextBytes(this IRandom random, byte[] buffer)
    {
        ThrowHelper.ThrowIfNull(buffer);
        NextBytes(random, buffer.AsSpan());
    }

    /// <summary>
    /// Fills the buffer with random bytes [0..0x7f].
    /// </summary>
    public static unsafe void NextBytes(this IRandom random, Span<byte> buffer)
    {
        if (random is Xoshiro128StarStarRandom xoshiro1)
        {
            FastNextBytes(ref xoshiro1.GetState(), buffer);
        }
        else if (random is SharedRandom shared && shared.GetLocalInstance() is Xoshiro128StarStarRandom xoshiro2)
        {
            FastNextBytes(ref xoshiro2.GetState(), buffer);
        }
        else
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)random.NextUInt();
            }
        }
    }

    static unsafe void FastNextBytes(ref Xoshiro128StarStar state, Span<byte> buffer)
    {
        uint s0 = state.S0, s1 = state.S1, s2 = state.S2, s3 = state.S3;

        while (buffer.Length >= sizeof(uint))
        {
            Unsafe.WriteUnaligned(
                ref MemoryMarshal.GetReference(buffer),
                BitOperations.RotateLeft(s1 * 5, 7) * 9);

            uint t = s1 << 9;
            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;
            s2 ^= t;
            s3 = BitOperations.RotateLeft(s3, 11);

            buffer = buffer[sizeof(uint)..];
        }

        if (!buffer.IsEmpty)
        {
            uint next = BitOperations.RotateLeft(s1 * 5, 7) * 9;
            byte* remainingBytes = (byte*)&next;

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = remainingBytes[i];
            }

            uint t = s1 << 9;
            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;
            s2 ^= t;
            s3 = BitOperations.RotateLeft(s3, 11);
        }

        state.S0 = s0;
        state.S1 = s1;
        state.S2 = s2;
        state.S3 = s3;
    }
}