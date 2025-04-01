using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NRandom.Algorithms;

/// <summary>
/// Implementation of Philox 4×32
/// </summary>
public unsafe struct Philox4x32(int rounds)
{
    fixed uint ctr[4];
    fixed uint key[2];

    public int Rounds => rounds;

    public Span<uint> Ctr => MemoryMarshal.CreateSpan(ref ctr[0], 4);
    public Span<uint> Key => MemoryMarshal.CreateSpan(ref key[0], 2);

    public void Init(uint seed0, uint seed1)
    {
        Ctr.Clear();
        key[0] = seed0;
        key[1] = seed1;
    }

    public void Next()
    {
        Next(Ctr, Key, rounds);
    }

    public static void Next(Span<uint> ctr, Span<uint> key, int rounds)
    {
        // Counter
        ctr[0]++;
        if (ctr[0] == 0)
        {
            ctr[1]++;
            if (ctr[1] == 0)
            {
                ctr[2]++;
                if (ctr[2] == 0)
                {
                    ctr[3]++;
                }
            }
        }

        for (int i = 0; i < rounds; i++)
        {
            // Round
            const uint MUL_0 = 0xD2511F53;
            const uint MUL_1 = 0xCD9E8D57;

            MulHiLo(MUL_0, ctr[0], out var hi0, out var lo0);
            MulHiLo(MUL_1, ctr[2], out var hi1, out var lo1);
            ctr[0] = hi1 ^ ctr[1] ^ key[0];
            ctr[2] = hi0 ^ ctr[3] ^ key[1];
            ctr[1] = lo1;
            ctr[3] = lo0;

            // Update Key
            const uint C0 = 0x9E3779B9;
            const uint C1 = 0xBB67AE85;

            key[0] = key[0] + C0;
            key[1] = key[1] + C1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void MulHiLo(uint lhs, uint rhs, out uint hi, out uint lo)
    {
        var product = (ulong)lhs * rhs;
        hi = (uint)(product >> 32);
        lo = (uint)product;
    }
}