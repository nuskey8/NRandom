namespace NRandom.Algorithms;

public unsafe struct TinyMt64(uint mat1, uint mat2, ulong tmat, bool linearityCheck = false)
{
    const int MIN_LOOP = 8;

    const int TINYMT64_MEXP = 127;
    const int TINYMT64_SH0 = 12;
    const int TINYMT64_SH1 = 11;
    const int TINYMT64_SH8 = 8;
    const ulong TINYMT64_MASK = 0x7fffffffffffffffUL;

    fixed ulong status[2];

    public void Init(ulong seed)
    {
        status[0] = seed ^ ((ulong)mat1 << 32);
        status[1] = mat2 ^ tmat;
        for (uint i = 1; i < MIN_LOOP; i++)
        {
            status[i & 1] ^= i + 6364136223846793005UL
                * (status[(i - 1) & 1]
                ^ (status[(i - 1) & 1] >> 62));
        }
        PeriodCertification();
    }

    public void Init(ReadOnlySpan<ulong> key)
    {
        const int lag = 1;
        const int mid = 1;
        const int size = 4;

        int i, j;
        int count;
        ulong r;
        Span<ulong> st = stackalloc ulong[4];

        st[0] = 0;
        st[1] = mat1;
        st[2] = mat2;
        st[3] = tmat;
        if (key.Length + 1 > MIN_LOOP)
        {
            count = key.Length + 1;
        }
        else
        {
            count = MIN_LOOP;
        }
        r = Init1(st[0] ^ st[mid % size] ^ st[(size - 1) % size]);
        st[mid % size] += r;
        r += (uint)key.Length;
        st[(mid + lag) % size] += r;
        st[0] = r;
        count--;
        for (i = 1, j = 0; (j < count) && (j < (uint) key.Length); j++) {
            r = Init1(st[i] ^ st[(i + mid) % size] ^ st[(i + size - 1) % size]);
            st[(i + mid) % size] += r;
            r += key[j] + (ulong)i;
            st[(i + mid + lag) % size] += r;
            st[i] = r;
            i = (i + 1) % size;
        }
        for (; j < count; j++)
        {
            r = Init1(st[i] ^ st[(i + mid) % size] ^ st[(i + size - 1) % size]);
            st[(i + mid) % size] += r;
            r += (ulong)i;
            st[(i + mid + lag) % size] += r;
            st[i] = r;
            i = (i + 1) % size;
        }
        for (j = 0; j < size; j++)
        {
            r = Init2(st[i] + st[(i + mid) % size] + st[(i + size - 1) % size]);
            st[(i + mid) % size] ^= r;
            r -= (ulong)i;
            st[(i + mid + lag) % size] ^= r;
            st[i] = r;
            i = (i + 1) % size;
        }
        status[0] = st[0] ^ st[1];
        status[1] = st[2] ^ st[3];
        PeriodCertification();
    }

    public ulong Next()
    {
        ulong x;

        status[0] &= TINYMT64_MASK;
        x = status[0] ^ status[1];
        x ^= x << TINYMT64_SH0;
        x ^= x >> 32;
        x ^= x << 32;
        x ^= x << TINYMT64_SH1;
        status[0] = status[1];
        status[1] = x;
        if ((x & 1) != 0)
        {
            status[0] ^= mat1;
            status[1] ^= (ulong)mat2 << 32;
        }

        return Temper();
    }

    void PeriodCertification()
    {
        if ((status[0] & TINYMT64_MASK) == 0 &&
            status[1] == 0)
        {
            status[0] = 'T';
            status[1] = 'M';
        }
    }

    ulong Temper()
    {
        ulong x;

        if (linearityCheck)
        {
            x = status[0] ^ status[1];
        }
        else
        {
            x = status[0] + status[1];
        }

        x ^= status[0] >> TINYMT64_SH8;
        if ((x & 1) != 0)
        {
            x ^= tmat;
        }

        return x;
    }


    static ulong Init1(ulong x)
    {
        return (x ^ (x >> 59)) * 2173292883993UL;
    }

    static ulong Init2(ulong x)
    {
        return (x ^ (x >> 59)) * 58885565329898161UL;
    }
}