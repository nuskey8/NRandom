using System.Numerics;
using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

// Original implementation: http://prng.di.unimi.it/xoshiro128starstar.c

/// <summary>
/// Implementation of xoshiro128**
/// </summary>
public record struct Xoshiro128StarStar(uint S0, uint S1, uint S2, uint S3)
{
    public Xoshiro128StarStar() : this(123456789, 362436069, 521288629, 88675123)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next()
    {
        uint s0 = S0, s1 = S1, s2 = S2, s3 = S3;

        uint result = BitOperations.RotateLeft(s1 * 5, 7) * 9;
        uint t = s1 << 9;

        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;

        s2 ^= t;
        s3 = BitOperations.RotateLeft(s3, 11);

        S0 = s0;
        S1 = s1;
        S2 = s2;
        S3 = s3;

        return result;
    }

    /// <summary>
    /// Equivalent to 2^64 calls to next()
    /// It can be used to generate 2^64 non-overlapping subsequences for parallel computations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Jump()
    {
        JumpCore(0x8764000b, 0xf542d2d3, 0x6fa035c3, 0x77f2db5b);
    }

    /// <summary>
    /// Equivalent to 2^192 calls to Next().
    /// It can be used to generate 2^64 starting points, from each of which Jump() will generate 2^64 non-overlapping subsequences for parallel distributed computations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LongJump()
    {
        JumpCore(0xb523952e, 0x0b6f099f3, 0xccf5a0ef, 0x1c580662);
    }

    void JumpCore(uint j0, uint j1, uint j2, uint j3)
    {
        uint s0 = 0, s1 = 0, s2 = 0, s3 = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int b = 0; b < 32; b++)
            {
                var j = i switch
                {
                    0 => j0,
                    1 => j1,
                    2 => j2,
                    3 => j3,
                    _ => default
                };

                if ((j & (1u << b)) != 0)
                {
                    s0 ^= S0;
                    s1 ^= S1;
                    s2 ^= S2;
                    s3 ^= S3;
                }

                Next();
            }
        }

        S0 = s0;
        S1 = s1;
        S2 = s2;
        S3 = s3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref uint s0, ref uint s1, ref uint s2, ref uint s3)
    {
        var xoshiro = new Xoshiro128StarStar(s0, s1, s2, s3);
        var result = xoshiro.Next();
        s0 = xoshiro.S0;
        s1 = xoshiro.S1;
        s2 = xoshiro.S2;
        s3 = xoshiro.S3;
        return result;
    }
}