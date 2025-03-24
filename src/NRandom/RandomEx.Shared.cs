namespace NRandom;

public static partial class RandomEx
{
    [ThreadStatic] static Random? seedGenerator;
    static Random SeedGenerator => seedGenerator ??= new();

    /// <summary>
    /// Provides a thread-safe IRandom instance that may be used concurrently from any thread.
    /// </summary>
    public static IRandom Shared { get; } = new SharedRandom();

    /// <summary>
    /// Creates an IRandom instance initialized with a random seed.
    /// </summary>
    public static IRandom Create()
    {
        return new Xoshiro256StarStarRandom((uint)SeedGenerator.Next());
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