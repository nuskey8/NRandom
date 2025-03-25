namespace NRandom.Collections;

public interface IWeightedCollection<T> : IReadOnlyCollection<WeightedValue<T>>
{
    T GetItem(IRandom random);
    void GetItems(IRandom random, Span<T> destination);
}