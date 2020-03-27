using System;
using System.Collections.Generic;
using System.Text;

public static class ArrayExtensions
{
    public static void Deconstruct<T>(this T[] self, out T a, out T b)
    {
        a = self.GetIndexOrDefault(0);
        b = self.GetIndexOrDefault(1);
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c)
    {
        a = self.GetIndexOrDefault(0);
        b = self.GetIndexOrDefault(1);
        c = self.GetIndexOrDefault(2);
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c, out T d)
    {
        a = self.GetIndexOrDefault(0);
        b = self.GetIndexOrDefault(1);
        c = self.GetIndexOrDefault(2);
        d = self.GetIndexOrDefault(3);
    }

    public static T GetIndexOrDefault<T>(this T[] self, int index, T defaultVal = default)
    {
        if (self.Length > index)
            return self[index];
        return defaultVal;
    }
}
