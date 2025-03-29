using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Tiny Mersenne Twister (64bit)
/// </summary>
public sealed class TinyMt64Random(uint mat1, uint mat2, ulong tmat, bool linearityCheck = false) : IRandom
{
    TinyMt64 mt = new(mat1, mat2, tmat, linearityCheck);

    public TinyMt64Random(bool linearityCheck = false) : this(0x8f7011ee, 0xfc78ff1f, 0x3793fdff, linearityCheck)
    {
    }

    public void InitState(uint seed)
    {
        mt.Init(seed);
    }

    public void InitState(ulong seed)
    {
        mt.Init(seed);
    }

    public void InitState(ReadOnlySpan<ulong> seed)
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