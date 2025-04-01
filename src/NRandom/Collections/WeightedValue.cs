using System.Diagnostics.CodeAnalysis;

namespace NRandom.Collections;

[Serializable]
public record struct WeightedValue<T>
{
    // These need to be fields for serialization in Unity.
    public T Value;
    public double Weight;

    public WeightedValue(T value, double weight)
    {
        Value = value;
        Weight = weight;
    }
}

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