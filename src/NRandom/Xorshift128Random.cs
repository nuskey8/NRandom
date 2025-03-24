using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using xorshift128
/// </summary>
public sealed class Xorshift128Random : IRandom
{
    Xorshift128 xorshift;

    public Xorshift128Random()
    {
        xorshift = new Xorshift128();
    }

    public Xorshift128Random(uint seed)
    {
        InitState(seed);
    }

    public Xorshift128Random(uint s0, uint s1, uint s2, uint s3)
    {
        xorshift = new Xorshift128(s0, s1, s2, s3);
    }

    public void InitState(uint seed)
    {
        do
        {
            xorshift.S0 = SplitMix32.Next(ref seed);
            xorshift.S1 = SplitMix32.Next(ref seed);
            xorshift.S2 = SplitMix32.Next(ref seed);
            xorshift.S3 = SplitMix32.Next(ref seed);
        } while (xorshift.S0 == 0 || xorshift.S1 == 0 || xorshift.S2 == 0 || xorshift.S3 == 0);
    }

    public uint NextUInt()
    {
        return xorshift.Next();
    }

    public ulong NextULong()
    {
        return (((ulong)xorshift.Next()) << 32) | xorshift.Next();
    }
}