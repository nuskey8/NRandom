using System.Diagnostics.CodeAnalysis;

namespace NRandom.Collections;

[Serializable]
public readonly record struct WeightedValue<T>(T Value, double Weight);

internal sealed class WeightedValueEqualityComparer<T> : IEqualityComparer<WeightedValue<T>>
{
    public static readonly WeightedValueEqualityComparer<T> Instance = new();

    public bool Equals(WeightedValue<T> x, WeightedValue<T> y)
    {
        return EqualityComparer<T>.Default.Equals(x.Value, y.Value);
    }

    public int GetHashCode([DisallowNull] WeightedValue<T> obj)
    {
        return EqualityComparer<T>.Default.GetHashCode(obj.Value!);
    }
}