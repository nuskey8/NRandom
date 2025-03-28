using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using SFC32
/// </summary>
public sealed class Sfc32Random : IRandom
{
    Sfc32 sfc;

    public void InitState(uint seed)
    {
        sfc.Init(seed);
    }

    public uint NextUInt()
    {
        return sfc.Next();
    }

    public ulong NextULong()
    {
        return (((ulong)sfc.Next()) << 32) | sfc.Next();
    }
}