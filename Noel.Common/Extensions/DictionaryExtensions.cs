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
}
