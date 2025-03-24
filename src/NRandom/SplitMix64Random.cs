using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using splitmix64
/// </summary>
public sealed class SplitMix64Random : IRandom
{
    SplitMix64 splitMix = new();

    public void InitState(uint seed)
    {
        splitMix.State = seed;
    }

    public uint NextUInt()
    {
        return (uint)(splitMix.Next() >> 32);
    }

    public ulong NextULong()
    {
        return splitMix.Next();
    }
}