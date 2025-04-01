using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

/// <summary>
/// Implementation of splitmix64
/// </summary>
public record struct SplitMix64(ulong State)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Next()
    {
        var z = State += 0x9e3779b97f4a7c15;
        z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        z = (z ^ (z >> 27)) * 0x94d049bb133111eb;
        return z ^ (z >> 31);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Next(ref ulong state)
    {
        var rand = new SplitMix64(state);
        var result = rand.Next();
        state = rand.State;
        return result;
    }
}