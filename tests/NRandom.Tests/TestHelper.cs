namespace NRandom.Tests;

public static class TestHelper
{
    public static IRandom[] CreateInstances()
    {
        return [
            new Pcg32Random(),
            new SplitMix32Random(),
            new SplitMix64Random(),
            new Xorshift32Random(),
            new Xorshift64Random(),
            new Xorshift128Random(),
            new Xoshiro128StarStarRandom(),
            new Xoshiro256StarStarRandom()
        ];
    }
}