namespace NRandom.Collections;

public static class WeightedCollectionExtensions
{
    public static T GetRandom<T>(this IWeightedCollection<T> collection)
    {
        return collection.GetRandom(RandomEx.Shared);
    }

    public static T GetRandom<T>(this IWeightedCollection<T> collection, IRandom random)
    {
        Span<T> buffer = [default!];
        collection.GetRandom(random, buffer);
        return buffer[0];
    }

    public static T[] GetRandom<T>(this IWeightedCollection<T> collection, int length, IRandom random)
    {
        ThrowHelper.ThrowIfLengthIsNegative(length);

        var array = new T[length];
        collection.GetRandom(random, array.AsSpan());

        return array;
    }

    public static T[] GetRandom<T>(this IWeightedCollection<T> collection, int length)
    {
        return GetRandom(collection, length, RandomEx.Shared);
    }

    public static void GetRandom<T>(this IWeightedCollection<T> collection, Span<T> destination)
    {
        collection.GetRandom(RandomEx.Shared, destination);
    }
}