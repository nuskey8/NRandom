using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Tiny Mersenne Twister (32bit)
/// </summary>
public sealed class TinyMt32Random : IRandom
{
    TinyMt32 mt;

    public TinyMt32Random(uint mat1, uint mat2, uint tmat, bool linearityCheck = false)
    {
        mt = new(mat1, mat2, tmat, linearityCheck);
        mt.Init(0x12345678);
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