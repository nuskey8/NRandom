using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using ChaCha
/// </summary>
public sealed class ChaChaRandom(int rounds = 8) : IRandom
{
    ChaCha chacha = new(rounds);
    readonly uint[] buffer = new uint[16];
    int bufferIndex = 16;

    public void InitState(uint seed)
    {
        Span<uint> k = stackalloc uint[12];
        for (int i = 0; i < k.Length; i++)
        {
            k[i] = SplitMix32.Next(ref seed);
        }
        chacha.Init(k[..8], k[8], k[9..]);
    }

    public uint NextUInt()
    {
        if (bufferIndex == 16)
        {
            chacha.Next(buffer);
            bufferIndex = 0;
        }

        return buffer[bufferIndex++];
    }

    public ulong NextULong()
    {
        return (((ulong)NextUInt()) << 32) | NextUInt();
    }
}