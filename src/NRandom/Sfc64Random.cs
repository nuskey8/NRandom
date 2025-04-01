using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using SFC64
/// </summary>
public sealed class Sfc64Random : IRandom
{
    Sfc64 sfc;

    public void InitState(uint seed)
    {
        sfc.Init(seed);
    }

    public uint NextUInt()
    {
        return (uint)(sfc.Next() >> 32);
    }

    public ulong NextULong()
    {
        return sfc.Next();
    }
}