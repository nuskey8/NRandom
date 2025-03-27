using NRandom.Algorithms;

namespace NRandom;

public sealed class ChaCha8Random : IRandom
{
    ChaCha8 chacha = new();
    readonly uint[] buffer = new uint[16];
    int bufferIndex = 16;

    public void InitState(uint seed)
    {
        chacha.Init(seed);
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