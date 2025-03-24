namespace NRandom.Collections;

public static class WeightedCollectionExtensions
{
    public static T GetItem<T>(this IWeightedCollection<T> collection)
    {
        return collection.GetItem(RandomEx.Shared);
    }

    public static T[] GetItems<T>(this IWeightedCollection<T> collection, int length, IRandom random)
    {
        ThrowHelper.ThrowIfLengthIsNegative(length);

        var array = new T[length];
        collection.GetItems(random, array.AsSpan());

        return array;
    }

    public static T[] GetItems<T>(this IWeightedCollection<T> collection, int length)
    {
        return GetItems(collection, length, RandomEx.Shared);
    }

    public static void GetItems<T>(this IWeightedCollection<T> collection, Span<T> destination)
    {
        collection.GetItems(RandomEx.Shared, destination);
    }
}