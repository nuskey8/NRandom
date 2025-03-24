using System.Runtime.CompilerServices;

namespace NRandom.Algorithms;

// Original implementation: https://www.pcg-random.org/download.html

/// <summary>
/// Implementation of PCG32 (PCG-XSH-RR)
/// </summary>
public record struct Pcg32
{
    const ulong Multipliar = 6364136223846793005UL;

    public ulong State { get; set; }
    public ulong Increment { get; set; }

    public Pcg32(ulong state, ulong stream)
    {
        State = state;
        Increment = (stream << 1) | 1;

        State += Increment;
        Next();
    }

    public Pcg32() : this(0xcafef00dd15ea5e5, 0xa02bdbf7bb3c0a7)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next()
    {
        ulong oldState = State;
        State = oldState * Multipliar + Increment;
        uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
        int rot = (int)(oldState >> 59);
        return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
    }
}