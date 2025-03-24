namespace NRandom;

public interface IRandom
{
    /// <summary>
    /// Returns a random uint value.
    /// </summary>
    uint NextUInt();

    /// <summary>
    /// Returns a random ulong value.
    /// </summary>
    ulong NextULong();

    /// <summary>
    /// Initialized the state of the instance with a given seed value.
    /// </summary>
    /// <param name="seed">The seed to initialize with.</param>
    void InitState(uint seed);
}