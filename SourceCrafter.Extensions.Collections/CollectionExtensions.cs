#nullable enable
using System.Collections.Generic;

namespace SourceCrafter;

public static class CollectionExtensions
{
    public static TList AddNested<TList, TKey, TValueItem>(this Dictionary<TKey, TList> listHash, TKey key, TValueItem valueItem)
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
