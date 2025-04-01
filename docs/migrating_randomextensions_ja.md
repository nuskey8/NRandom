# RandomExtensions(v1)からの移行

RandomExtensionsはv2にてNRandomに名称を変更しました。また、APIにいくつかの破壊的変更が含まれています。ここではRandomExtensionsからの変更点を記載します。

## 名前空間の変更

使用する名前空間が`RandomExtensions`から`NRandom`に変更されました。

```cs
// RandomExtensions(v1)
using RandomExtensions;

// NRandom(v2)
using NRandom;
```

## `rand.GetItem()`の削除

ランダムな要素を一つ取得する`GetItem()`が削除されました。代わりに`NRandom.Linq`の`RandomElement()`を利用してください。

```cs
IRandom rand;
T[] array;

// RandomExtensions(v1)
T result = rand.GetItem(array);

// NRandom(v2)
T result = array.RandomElement();
```

## `IWeightedCollection<T>.GetItem()`/`GetItems()` -> `GetRandom()`

`IWeightedCollection<T>`の`GetItem()`/`GetItems()`は`GetRandom()`に変更されました。

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