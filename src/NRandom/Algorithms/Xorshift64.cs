using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

/// <summary>
/// Implementation of xorshift64
/// </summary>
public record struct Xorshift64(ulong State)
{
    public Xorshift64() : this(0x123456789ABCDEF)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Next()
    {
        ulong x = State;
        x ^= x << 13;
        x ^= x >> 7;
        x ^= x << 17;
        return State = x;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Next(ref ulong state)
    {
        var xorshift = new Xorshift64(state);
        var result = xorshift.Next();
        state = xorshift.State;
        return result;
    }
}