using System.Collections;

namespace NRandom.Collections;

public interface IReadOnlyWeightedList<T> : IWeightedCollection<T>, IReadOnlyList<WeightedValue<T>>
{
}

public class WeightedList<T> : IReadOnlyWeightedList<T>, IList<WeightedValue<T>>
{
    public sealed class ValueCollection(WeightedList<T> list) : IReadOnlyList<T>
    {
        public T this[int index] => list[index].Value;
        public int Count => list.Count;
        public bool IsReadOnly => true;

        public Enumerator GetEnumerator()
        {
            return new Enumerator(list);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator(WeightedList<T> list) : IEnumerator<T>
        {
            int offset;
            T? current;

            public T Current => current!;
            object? IEnumerator.Current => current;

            public bool MoveNext()
            {
                if (offset == list.Count) return false;
                current = list[offset].Value;
                offset++;
                return true;
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }

    public WeightedList(int capacity)
    {
        list = new(capacity);
        values = new(this);
    }

    public WeightedList()
    {
        list = [];
        values = new(this);
    }

    double totalWeight;
    readonly List<WeightedValue<T>> list;
    readonly ValueCollection values;

    internal List<WeightedValue<T>> GetListInternal() => list;
    internal void SetTotalWeightInternal(double totalWeight) => this.totalWeight = totalWeight;

    public double TotalWeight => totalWeight;

    public WeightedValue<T> this[int index]
    {
        get => list[index];
        set
        {
            totalWeight -= list[index].Weight;
            list[index] = value;
            totalWeight += value.Weight;
        }
    }

    public int Count => list.Count;
    public bool IsReadOnly => false;

    public ValueCollection Values => values;

    public void Add(WeightedValue<T> item)
    {
        list.Add(item);
        totalWeight += item.Weight;
    }

    public void Add(T value, double weight)
    {
        Add(new WeightedValue<T>(value, weight));
    }

    public void Clear()
    {
        list.Clear();
    }

    public bool Contains(WeightedValue<T> item)
    {
        return list.Contains(item);
    }

    public bool Contains(T item)
    {
        return IndexOf(item) != -1;
    }

    public void CopyTo(WeightedValue<T>[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex);
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    public void GetRandom(IRandom random, Span<T> destination)
    {
        if (list.Count == 0) throw new InvalidOperationException("Empty list");

        for (int n = 0; n < destination.Length; n++)
        {
            var r = random.NextDouble() * totalWeight;
            var current = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                current += list[i].Weight;
                if (r <= current)
                {
                    destination[n] = list[i].Value;
                    continue;
                }
            }

            destination[n] = list[^1].Value;
        }
    }

    public int IndexOf(WeightedValue<T> item)
    {
        return list.IndexOf(item);
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(list[i].Value, item)) return i;
        }

        return -1;
    }

    public void Insert(int index, WeightedValue<T> item)
    {
        list.Insert(index, item);
        totalWeight += item.Weight;
    }

    public void Insert(int index, T item, double weight)
    {
        Insert(index, new WeightedValue<T>(item, weight));
    }

    public bool Remove(WeightedValue<T> item)
    {
        if (list.Remove(item))
        {
            totalWeight -= item.Weight;
            return true;
        }

        return false;
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index == -1) return false;

        RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        var value = list[index];
        list.RemoveAt(index);
        totalWeight -= value.Weight;
    }

    public void RemoveRandom(out T item)
    {
        RemoveRandom(RandomEx.Shared, out item);
    }

    public void RemoveRandom(IRandom random, out T item)
    {
        if (list.Count == 0) throw new InvalidOperationException("Empty list");

        var r = random.NextDouble() * totalWeight;
        var current = 0.0;

        for (int i = 0; i < list.Count; i++)
        {
            var wv = list[i];
            current += wv.Weight;

            if (r <= current)
            {
                item = wv.Value;
                list.RemoveAt(i);
                totalWeight -= wv.Weight;
                return;
            }
        }

        var lastIndex = list.Count - 1;
        var lastWv = list[lastIndex];
        item = lastWv.Value;
        list.RemoveAt(lastIndex);
        totalWeight -= lastWv.Weight;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<WeightedValue<T>> IEnumerable<WeightedValue<T>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator(WeightedList<T> list) : IEnumerator<WeightedValue<T>>
    {
        int offset;
        WeightedValue<T> current;

        public WeightedValue<T> Current => current;
        object? IEnumerator.Current => current;

        public bool MoveNext()
        {
            if (offset == list.Count) return false;
            current = list[offset];
            offset++;
            return true;
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}