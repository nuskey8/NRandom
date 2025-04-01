using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using PCG32
/// </summary>
public sealed class Pcg32Random : IRandom
{
    Pcg32 pcg = new();

    public void InitState(uint seed)
    {
        pcg = new Pcg32();
        pcg.State += seed;
        pcg.Next();
    }

    public uint NextUInt()
    {
        return pcg.Next();
    }

    public ulong NextULong()
    {
        return (((ulong)pcg.Next()) << 32) | pcg.Next();
    }
}