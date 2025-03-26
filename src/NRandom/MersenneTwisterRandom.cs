using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Mersenne Twister (MT19937)
/// </summary>
public sealed class MersenneTwisterRandom : IRandom
{
    readonly MersenneTwister mt = new();

    public void InitState(uint seed)
    {
        mt.Init(seed);
    }

    public void InitState(ulong seed)
    {
        mt.Init(seed);
    }

    public uint NextUInt()
    {
        return (uint)(mt.Next() >> 32);
    }

    public ulong NextULong()
    {
        return mt.Next();
    }
}