#nullable enable
using System.Collections.Generic;

namespace SourceCrafter;

public static class CollectionExtensions
{
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey key, out TValue val) 
        => (key, val) = (source.Key, source.Value);

    public static TList AddNested<TList, TKey, TValueItem>(this Dictionary<TKey, TList> listHash, TKey key, TValueItem valueItem)
        where TKey : notnull
        where TList : ICollection<TValueItem>, new()
    {
        if (listHash.TryGetValue(key, out var valueItems))
            valueItems.Add(valueItem);
        else
            listHash.Add(key, valueItems = new() { valueItem });
        return valueItems;
    }

    public static T[] Collect<T>(params T[] items) => items;
}
