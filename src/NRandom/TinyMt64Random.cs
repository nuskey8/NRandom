using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Tiny Mersenne Twister (64bit)
/// </summary>
public sealed class TinyMt64Random : IRandom
{
    TinyMt64 mt;

    public TinyMt64Random(uint mat1, uint mat2, ulong tmat, bool linearityCheck = false)
    {
        mt = new(mat1, mat2, tmat, linearityCheck);
        mt.Init(0x123456789ABCDEF);
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