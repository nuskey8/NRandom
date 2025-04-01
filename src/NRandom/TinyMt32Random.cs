using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Tiny Mersenne Twister (32bit)
/// </summary>
public sealed class TinyMt32Random(uint mat1, uint mat2, uint tmat, bool linearityCheck = false) : IRandom
{
    TinyMt32 mt = new(mat1, mat2, tmat, linearityCheck);

    public TinyMt32Random(bool linearityCheck = false) : this(0x8f7011ee, 0xfc78ff1f, 0x3793fdff, linearityCheck)
    {
    }

    public void InitState(uint seed)
    {
        mt.Init(seed);
    }

    public void InitState(ReadOnlySpan<uint> seed)
    {
        mt.Init(seed);
    }

    public uint NextUInt()
    {
        return mt.Next();
    }

    public ulong NextULong()
    {
        return (((ulong)mt.Next()) << 32) | mt.Next();
    }
}