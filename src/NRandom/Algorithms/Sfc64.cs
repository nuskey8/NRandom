namespace NRandom.Algorithms;

/// <summary>
/// Implementation of SFC64
/// </summary>
public record struct Sfc64(ulong A, ulong B, ulong C, ulong Counter)
{
    public void Init(ulong seed)
    {
        Init(seed, seed, seed);
    }

    public void Init(ulong a, ulong b, ulong c)
    {
        A = a;
        B = b;
        C = c;
        Counter = 1;
        for (int i = 0; i < 12; i++) Next();
    }

    public ulong Next()
    {
        const int BARREL_SHIFT = 24;
        const int RSHIFT = 11;
        const int LSHIFT = 3;

        var tmp = A + B + Counter++;
        A = B ^ (B >> RSHIFT);
        B = C + (C << LSHIFT);
        C = ((C << BARREL_SHIFT) | (C >> (64 - BARREL_SHIFT))) + tmp;

        return tmp;
    }
}