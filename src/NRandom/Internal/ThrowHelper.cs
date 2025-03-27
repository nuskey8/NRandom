namespace NRandom;

internal static class ThrowHelper
{
    public const string CollectionIsEmpty = "The collection is empty";
    public const string MaxMustBePositive = "max must be positive";
    public const string MinMustBeLessThanOrEqualToMax = "min must be less than or equal to max";

    public static void ThrowIfNull<T>(T value)
    {
        if (value == null) throw new ArgumentNullException();
    }

    public static void ThrowIfEmpty<T>(ReadOnlySpan<T> span)
    {
        if (span.IsEmpty) throw new ArgumentException(CollectionIsEmpty);
    }

    public static void ThrowIfEmpty<T>(IReadOnlyList<T> list)
    {
        if (list.Count == 0) throw new ArgumentException(CollectionIsEmpty);
    }

    public static void ThrowIfLengthIsNegative(int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be less than zero");
    }

    public static void CheckMax(int max)
    {
        if (max < 0) throw new ArgumentException(MaxMustBePositive);
    }

    public static void CheckMax(long max)
    {
        if (max < 0) throw new ArgumentException(MaxMustBePositive);
    }

    public static void CheckMax(float max)
    {
        if (max < 0) throw new ArgumentException(MaxMustBePositive);
    }

    public static void CheckMax(double max)
    {
        if (max < 0) throw new ArgumentException(MaxMustBePositive);
    }

    public static void CheckMinMax(int min, int max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }

    public static void CheckMinMax(uint min, uint max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }

    public static void CheckMinMax(long min, long max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }

    public static void CheckMinMax(ulong min, ulong max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }

    public static void CheckMinMax(float min, float max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }

    public static void CheckMinMax(double min, double max)
    {
        if (min > max) throw new ArgumentException(MinMustBeLessThanOrEqualToMax);
    }
}