using System;
using System.Collections.Generic;
using System.Text;

public static class DictionaryExtensions
{
    public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue defaultVal = default)
    {
        if (self.TryGetValue(key, out var value))
            return value;

        return defaultVal;
    }

    public static TValue Establish<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key) where TValue : new()
    {
        if (!self.TryGetValue(key, out var value))
            self[key] = value = new TValue();

        return value;
    }

    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> self, out TKey key, out TValue value)
    {
        key = self.Key;
        value = self.Value;
    }
}
