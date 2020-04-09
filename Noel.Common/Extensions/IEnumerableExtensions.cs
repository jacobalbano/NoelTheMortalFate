using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IEnumerableExtensions
{
    public static IEnumerable<ProgressItem<T>> Progress<T>(this IList<T> self)
    {
        for (int i = 0; i < self.Count; ++i)
            yield return new ProgressItem<T>(self[i], i + 1, self.Count);
    }

    public static IEnumerable<ProgressItem<T>> Progress<T>(this IReadOnlyList<T> self)
    {
        for (int i = 0; i < self.Count; ++i)
            yield return new ProgressItem<T>(self[i], i + 1, self.Count);
    }

    public struct ProgressItem<T>
    {
        public ProgressItem(T value, int number, int total)
        {
            Value = value;
            Number = number;
            Total = total;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }

        public T Value { get; }
        public int Number { get; }
        public int Total { get; }

        public static implicit operator T(ProgressItem<T> item) => item.Value;

        public override bool Equals(object obj)
        {
            return obj is ProgressItem<T> i && i.Value?.Equals(Value) == true;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ProgressItem<T> left, ProgressItem<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProgressItem<T> left, ProgressItem<T> right)
        {
            return !(left == right);
        }
    }
}