using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using xorshift32
/// </summary>
public sealed class Xorshift32Random : IRandom
{
    Xorshift32 xorshift = new();

    public void InitState(uint seed)
    {
        xorshift.State = seed;
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