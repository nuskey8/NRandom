namespace NRandom.Collections;

public interface IWeightedCollection<T> : IReadOnlyCollection<WeightedValue<T>>
{
    void GetRandom(IRandom random, Span<T> destination);
}