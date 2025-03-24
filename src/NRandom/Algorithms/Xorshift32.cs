using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

/// <summary>
/// Implementation of xorshift32
/// </summary>
public record struct Xorshift32(uint State)
{
    public Xorshift32() : this(0x12345678)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next()
    {
        uint x = State;
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;
        return State = x;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref uint state)
    {
        var xorshift = new Xorshift32(state);
        var result = xorshift.Next();
        state = xorshift.State;
        return result;
    }
}