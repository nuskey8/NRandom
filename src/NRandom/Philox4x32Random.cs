using NRandom.Algorithms;

namespace NRandom;

/// <summary>
/// IRandom implementation using Philox 4x32
/// </summary>
public sealed class Philox4x32Random(int rounds = 10) : IRandom
{
    Philox4x32 philox = new(rounds);
    int index = 4;

    public void InitState(uint seed)
    {
        philox.Init(seed, SplitMix32.Next(ref seed));
    }

    public uint NextUInt()
    {
        if (index == 4)
        {
            philox.Next();
            index = 0;
        }

        return philox.Ctr[index++];
    }

    public ulong NextULong()
    {
        return (((ulong)NextUInt()) << 32) | NextUInt();
    }
}