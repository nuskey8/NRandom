using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.CollectionsMarshal;

#if !NET8_0_OR_GREATER
#pragma warning disable

namespace System.Runtime.InteropServices
{
    internal static class CollectionsMarshal
    {
        internal static readonly int ListSize;

        static CollectionsMarshal()
        {
            try
            {
                ListSize = typeof(List<>).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Length;
            }
            catch
            {
                ListSize = 3;
            }
        }

#if !NET8_0_OR_GREATER
        internal static Span<T> AsSpan<T>(this List<T>? list)
        {
            Span<T> span = default;
            if (list is not null)
            {
                if (ListSize == 3)
                {
                    var view = Unsafe.As<ListViewA<T>>(list);
                    T[] items = view._items;
                    span = items.AsSpan(0, list.Count);
                }
                else if (ListSize == 4)
                {
                    var view = Unsafe.As<ListViewB<T>>(list);
                    T[] items = view._items;
                    span = items.AsSpan(0, list.Count);
                }
            }

            return span;
        }
#endif

        // This is not polyfill.
        // Unlike the original SetCount, this does not grow if the count is smaller.
        // Therefore, the internal collection size of the List must always be greater than or equal to the count.
        internal static void UnsafeSetCount<T>(this List<T>? list, int count)
        {
            if (list is not null)
            {
                // Unsafe.As<>._size is failed in Unity so don't use it.
                var collection = FillCollection<T>.Instance;
                if (collection == null)
                {
                    collection = FillCollection<T>.Instance = new FillCollection<T>(0);
                }
                collection.Count = count;

                list.AddRange(collection);
            }
        }

        // AddRange uses CopyTo only.
        internal sealed class FillCollection<T>(int count) : ICollection<T>
        {
            [ThreadStatic]
            public static FillCollection<T>? Instance;

            public int Count { get; set; } = count; // mutable

            public bool IsReadOnly => true;

            public void CopyTo(T[] array, int arrayIndex)
            {
                // do nothing.
            }

            public void Add(T item)
            {
            }

            public void Clear()
            {
            }

            public bool Contains(T item)
            {
                return true;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (var i = 0; i < count; i++)
                {
                    yield return default(T);
                }
            }

            public bool Remove(T item)
            {
                return true;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    internal class ListViewA<T>
    {
        public T[] _items;
        public int _size;
        public int _version;
    }

    internal class ListViewB<T>
    {
        public T[] _items;
        public int _size;
        public int _version;
        private Object _syncRoot; // in .NET Framework
    }
}

#endif