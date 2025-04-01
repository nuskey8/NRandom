namespace NRandom.Algorithms;

// Original implementation: https://github.com/MersenneTwister-Lab/TinyMT/blob/master/tinymt/tinymt32.c

/// <summary>
/// Implementation of Tiny Mersenne Twister (TinyMT)
/// </summary>
public unsafe struct TinyMt32(uint mat1, uint mat2, uint tmat, bool linearityCheck = false)
{
    const int MIN_LOOP = 8;
    const int PRE_LOOP = 8;

    const int TINYMT32_SH0 = 1;
    const int TINYMT32_SH1 = 10;
    const int TINYMT32_SH8 = 8;
    const uint TINYMT32_MASK = 0x7fffffffu;

    fixed uint status[4];

    public void Init(uint seed)
    {
        status[0] = seed;
        status[1] = mat1;
        status[2] = mat2;
        status[3] = tmat;

        for (uint i = 1; i < MIN_LOOP; i++)
        {
            status[i & 3] ^= i + 1812433253u
                * (status[(i - 1) & 3]
                ^ (status[(i - 1) & 3] >> 30));
        }

        PeriodCertification();

        for (int i = 0; i < PRE_LOOP; i++)
        {
            Next();
        }
    }

    public void Init(ReadOnlySpan<uint> key)
    {
        const uint lag = 1;
        const uint mid = 1;
        const uint size = 4;
        uint i, j;
        uint count;
        uint r;

        status[0] = 0;
        status[1] = mat1;
        status[2] = mat2;
        status[3] = tmat;
        if (key.Length + 1 > MIN_LOOP)
        {
            count = (uint)key.Length + 1;
        }
        else
        {
            count = MIN_LOOP;
        }
        r = Init1(status[0] ^ status[mid % size] ^ status[(size - 1) % size]);
        status[mid % size] += r;
        r += (uint)key.Length;
        status[(mid + lag) % size] += r;
        status[0] = r;
        count--;
        for (i = 1, j = 0; (j < count) && (j < (uint)key.Length); j++)
        {
            r = Init1(status[i % size]
                ^ status[(i + mid) % size]
                ^ status[(i + size - 1) % size]);
            status[(i + mid) % size] += r;
            r += key[(int)j] + i;
            status[(i + mid + lag) % size] += r;
            status[i % size] = r;
            i = (i + 1) % size;
        }
        for (; j < count; j++)
        {
            r = Init1(status[i % size]
                ^ status[(i + mid) % size]
                ^ status[(i + size - 1) % size]);
            status[(i + mid) % size] += r;
            r += i;
            status[(i + mid + lag) % size] += r;
            status[i % size] = r;
            i = (i + 1) % size;
        }
        for (j = 0; j < size; j++)
        {
            r = Init2(status[i % size]
                + status[(i + mid) % size]
                + status[(i + size - 1) % size]);
            status[(i + mid) % size] ^= r;
            r -= i;
            status[(i + mid + lag) % size] ^= r;
            status[i % size] = r;
            i = (i + 1) % size;
        }
        PeriodCertification();
        for (i = 0; i < PRE_LOOP; i++)
        {
            Next();
        }
    }

    public uint Next()
    {
        uint x, y;

        y = status[3];
        x = (status[0] & TINYMT32_MASK)
            ^ status[1]
            ^ status[2];
        x ^= x << TINYMT32_SH0;
        y ^= (y >> TINYMT32_SH0) ^ x;
        status[0] = status[1];
        status[1] = status[2];
        status[2] = x ^ (y << TINYMT32_SH1);
        status[3] = y;
        var a = -(int)(y & 1) & (int)mat1;
        var b = -(int)(y & 1) & (int)mat2;
        status[1] ^= (uint)a;
        status[2] ^= (uint)b;

        return Temper();
    }

    void PeriodCertification()
    {
        if ((status[0] & TINYMT32_MASK) == 0 &&
            status[1] == 0 &&
            status[2] == 0 &&
            status[3] == 0)
        {
            status[0] = 'T';
            status[1] = 'I';
            status[2] = 'N';
            status[3] = 'Y';
        }
    }

    uint Temper()
    {
        uint t0, t1;
        t0 = status[3];
        if (linearityCheck)
        {
            t1 = status[0] ^ (status[2] >> TINYMT32_SH8);
        }
        else
        {
            t1 = status[0] + (status[2] >> TINYMT32_SH8);
        }
        t0 ^= t1;
        if ((t1 & 1) != 0)
        {
            t0 ^= tmat;
        }
        return t0;
    }

    static uint Init1(uint x)
    {
        return (x ^ (x >> 27)) * 1664525u;
    }

    static uint Init2(uint x)
    {
        return (x ^ (x >> 27)) * 1566083941u;
    }
}