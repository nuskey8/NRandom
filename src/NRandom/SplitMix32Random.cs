using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using splitmix32
/// </summary>
public sealed class SplitMix32Random : IRandom
{
    SplitMix32 splitMix = new();

    public void InitState(uint seed)
    {
        splitMix.State = seed;
    }

    public uint NextUInt()
    {
        return splitMix.Next();
    }

    public ulong NextULong()
    {
        return (((ulong)splitMix.Next()) << 32) | splitMix.Next();
    }
}