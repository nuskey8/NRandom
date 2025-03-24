using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

/// <summary>
/// Implementation of splitmix32
/// </summary>
public record struct SplitMix32(uint State)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next()
    {
        var z = State += 0x9E3779B9;
        z = (z ^ (z >> 16)) * 0x85EBCA6B;
        z = (z ^ (z >> 13)) * 0xC2B2AE35;
        return z ^ (z >> 16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref uint state)
    {
        var rand = new SplitMix32(state);
        var result = rand.Next();
        state = rand.State;
        return result;
    }
}