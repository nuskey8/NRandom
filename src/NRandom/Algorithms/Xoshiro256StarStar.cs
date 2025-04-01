using System.Numerics;
using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

// Original implementation: http://prng.di.unimi.it/xoshiro256starstar.c

/// <summary>
/// Implementation of xoshiro256**
/// </summary>
public record struct Xoshiro256StarStar(ulong S0, ulong S1, ulong S2, ulong S3)
{
    public Xoshiro256StarStar() : this(1234567890123456789, 3624360693624360693, 521288629521288629, 886751238867512389)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Next()
    {
        ulong s0 = S0, s1 = S1, s2 = S2, s3 = S3;

        ulong result = BitOperations.RotateLeft(s1 * 5, 7) * 9;
        ulong t = s1 << 9;

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
    /// Equivalent to 2^128 calls to Next()
    /// It can be used to generate 2^128 non-overlapping subsequences for parallel computations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Jump()
    {
        JumpCore(0x180ec6d33cfd0aba, 0xd5a61266f0c9392c, 0xa9582618e03fc9aa, 0x39abdc4529b1661c);
    }

    /// <summary>
    /// Equivalent to 2^192 calls to Next()
    /// It can be used to generate 2^64 starting points, from each of which Jump() will generate 2^64 non-overlapping subsequences for parallel distributed computations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LongJump()
    {
        JumpCore(0x76e15d3efefdcbbf, 0xc5004e441c522fb3, 0x77710069854ee241, 0x39109bb02acbe635);
    }

    void JumpCore(ulong j0, ulong j1, ulong j2, ulong j3)
    {
        ulong s0 = 0, s1 = 0, s2 = 0, s3 = 0;

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

                if ((j & (1ul << b)) != 0)
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
    public static ulong Next(ref ulong s0, ref ulong s1, ref ulong s2, ref ulong s3)
    {
        var xoshiro = new Xoshiro256StarStar(s0, s1, s2, s3);
        var result = xoshiro.Next();
        s0 = xoshiro.S0;
        s1 = xoshiro.S1;
        s2 = xoshiro.S2;
        s3 = xoshiro.S3;
        return result;
    }
}