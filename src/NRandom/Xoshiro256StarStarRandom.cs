using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using xoshiro256**
/// </summary>
public sealed class Xoshiro256StarStarRandom : IRandom
{
    Xoshiro256StarStar xoshiro;

    public Xoshiro256StarStarRandom()
    {
        xoshiro = new Xoshiro256StarStar();
    }

    public Xoshiro256StarStarRandom(uint s0, uint s1, uint s2, uint s3)
    {
        xoshiro = new Xoshiro256StarStar(s0, s1, s2, s3);
    }

    public void InitState(uint seed)
    {
        var s = (ulong)seed;

        do
        {
            xoshiro.S0 = SplitMix64.Next(ref s);
            xoshiro.S1 = SplitMix64.Next(ref s);
            xoshiro.S2 = SplitMix64.Next(ref s);
            xoshiro.S3 = SplitMix64.Next(ref s);
        } while (xoshiro.S0 == 0 && xoshiro.S1 == 0 && xoshiro.S2 == 0 && xoshiro.S3 == 0);
    }

    public uint NextUInt()
    {
        return (uint)(xoshiro.Next() >> 32);
    }

    public ulong NextULong()
    {
        return xoshiro.Next();
    }

    public void Jump()
    {
        xoshiro.Jump();
    }

    public void LongJump()
    {
        xoshiro.LongJump();
    }

    internal ref Xoshiro256StarStar GetState()
    {
        return ref xoshiro;
    }
}