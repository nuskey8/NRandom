#if !NET6_0_OR_GREATER

using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

internal static class CollectionsMarshal
{
    public static Span<T> AsSpan<T>(List<T> list)
    {
        ref var view = ref Unsafe.As<List<T>, ListView<T>>(ref list);
        return view._items.AsSpan(0, view._size);
    }

    internal sealed class ListView<T>
    {
        public T[] _items = default!;
        public int _size;
        public int _version;
    }
}

#endif