using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using xorshift64
/// </summary>
public sealed class Xorshift64Random : IRandom
{
    Xorshift64 xorshift = new();

    public void InitState(uint seed)
    {
        xorshift.State = seed;
    }

    public uint NextUInt()
    {
        return (uint)(xorshift.Next() >> 32);
    }

    public ulong NextULong()
    {
        return xorshift.Next();
    }
}