using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtensions
{
    public static TValue PickRandom<TValue>(this IReadOnlyList<TValue> list)
    {
        if (list.Count == 0)
        {
            return default(TValue);
        }

        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static void Dispose<TValue>(this ICollection<TValue> collection) where TValue : IDisposable
    {
        if (collection.Count == 0)
        {
            return;
        }

        foreach (var value in collection)
        {
            value.Dispose();
        }

        collection.Clear();
    }

    public static void Dispose<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TValue : IDisposable
    {
        if (dictionary.Count == 0)
        {
            return;
        }

        foreach (var value in dictionary.Values)
        {
            value.Dispose();
        }

        dictionary.Clear();
    }
}
