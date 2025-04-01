namespace NRandom.Algorithms;

// Original Implementation: https://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/MT2002/CODES/mt19937ar.c

/// <summary>
/// Implementation of Mersenne Twister (MT19937)
/// </summary>
public sealed class MersenneTwister
{
    const int N = 624;
    const int M = 397;
    const ulong MATRIX_A = 0x9908b0dfUL;
    const ulong UPPER_MASK = 0x80000000UL;
    const ulong LOWER_MASK = 0x7fffffffUL;

    readonly ulong[] mt = new ulong[N];
    int mti = N + 1;

    public void Init(ulong seed)
    {
        mt[0] = seed & 0xffffffffUL;
        for (mti = 1; mti < N; mti++)
        {
            mt[mti] = 1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + (ulong)mti;
            mt[mti] &= 0xffffffffUL;
        }
    }

    public uint Next()
    {
        ulong y;
        ReadOnlySpan<ulong> mag01 = [0x0UL, MATRIX_A];

        if (mti >= N)
        {
            int kk;

            if (mti == N + 1) Init(5489UL);

            for (kk = 0; kk < N - M; kk++)
            {
                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[(int)(y & 0x1UL)];
            }
            for (; kk < N - 1; kk++)
            {
                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[(int)(y & 0x1UL)];
            }
            y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
            mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[(int)(y & 0x1UL)];

            mti = 0;
        }

        y = mt[mti++];

        y ^= (y >> 11);
        y ^= (y << 7) & 0x9d2c5680UL;
        y ^= (y << 15) & 0xefc60000UL;
        y ^= (y >> 18);

        return (uint)y;
    }

}