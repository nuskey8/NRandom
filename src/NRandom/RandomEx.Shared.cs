using System.Security.Cryptography;

namespace NRandom;

public static partial class RandomEx
{
    /// <summary>
    /// Provides a thread-safe IRandom instance that may be used concurrently from any thread.
    /// </summary>
    public static IRandom Shared { get; } = new SharedRandom();

    /// <summary>
    /// Creates an IRandom instance initialized with a random seed.
    /// </summary>
    public static IRandom Create()
    {
        var instance = new Xoshiro256StarStarRandom();
        instance.InitState((uint)RandomNumberGenerator.GetInt32(int.MaxValue));
        return instance;
    }
}

internal sealed class SharedRandom : IRandom
{
    [ThreadStatic] static IRandom? random;
    static IRandom LocalRandom => random ?? Create();

    static IRandom Create()
    {
        return random = RandomEx.Create();
    }

    public void InitState(uint seed)
    {
        LocalRandom.InitState(seed);
    }

    public uint NextUInt()
    {
        return LocalRandom.NextUInt();
    }

    public ulong NextULong()
    {
        return LocalRandom.NextULong();
    }

    internal IRandom GetLocalInstance()
    {
        return LocalRandom;
    }
}