# Migration from RandomExtensions(v1)

The `RandomExtensions` has been changed to `NRandom` in v2. Additionally, there are several breaking changes in the API. Below are the changes from RandomExtensions.

## Namespace Change

The namespace has changed from `RandomExtensions` to `NRandom`.

```cs
// RandomExtensions(v1)
using RandomExtensions;

// NRandom(v2)
using NRandom;
```

## Removal of `rand.GetItem()`

The method `GetItem()`, which was used to get a random element, has been removed. Instead, use `RandomElement()` from `NRandom.Linq`.

```cs
IRandom rand;
T[] array;

// RandomExtensions(v1)
T result = rand.GetItem(array);

// NRandom(v2)
T result = array.RandomElement();
```

## `IWeightedCollection<T>.GetItem()`/`GetItems()` -> `GetRandom()`

The methods `GetItem()`/`GetItems()` of `IWeightedCollection<T>` have been renamed to `GetRandom()`.

```cs
WeightedList<T> list;
Span<T> buffer;

// RandomExtensions(v1)
list.GetItem();
list.GetItems(buffer);

// NRandom(v2)
list.GetRandom();
list.GetRandom(buffer);
```