namespace NRandom.Algorithms;

/// <summary>
/// Implementation of SFC32
/// </summary>
public record struct Sfc32(uint A, uint B, uint C, uint Counter)
{
    public void Init(uint seed)
    {
        Init(0, seed, seed >> 32);
    }

    public void Init(uint a, uint b, uint c)
    {
        A = a;
        B = b;
        C = c;
        Counter = 1;
        for (int i = 0; i < 15; i++) Next();
    }

    public uint Next()
    {
        const int BARREL_SHIFT = 21;
        const int RSHIFT = 9;
        const int LSHIFT = 3;

        var tmp = A + B + Counter++;
        A = B ^ (B >> RSHIFT);
        B = C + (C << LSHIFT);
        C = ((C << BARREL_SHIFT) | (C >> (32 - BARREL_SHIFT))) + tmp;

        return tmp;
    }
}