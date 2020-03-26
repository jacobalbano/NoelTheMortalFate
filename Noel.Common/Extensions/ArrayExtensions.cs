using System;
using System.Collections.Generic;
using System.Text;

public static class ArrayExtensions
{
    public static void Deconstruct<T>(this T[] self, out T a, out T b)
    {
        a = self[0];
        b = self[1];
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c)
    {
        a = self[0];
        b = self[1];
        c = self[2];
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c, out T d)
    {
        a = self[0];
        b = self[1];
        c = self[2];
        d = self[3];
    }
}
