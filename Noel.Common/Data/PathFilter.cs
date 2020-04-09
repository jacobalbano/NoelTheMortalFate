using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Noel.Common.Data
{
    public class PathFilter
    {
        public static PathFilter Parse(string pattern)
        {
            var parts = pattern.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var conditions = new List<Tuple<int, Condition[]>>();

            int fromBottom = 0;
            for (int i = parts.Length; i --> 0;)
            {
                fromBottom++;
                var part = parts[i];
                var conditionStart = part.IndexOf('(');
                if (conditionStart >= 0)
                {
                    conditions.Add(Tuple.Create(fromBottom, part.Substring(conditionStart)
                        .Trim('(', ')')
                        .Split('&')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(Condition.Parse)
                        .ToArray()));

                    parts[i] = part.Substring(0, conditionStart);
                }
            }

            return new PathFilter(
                string.Join(".", parts),
                conditions
            );
        }

        private readonly string RoughPass;
        private readonly List<Tuple<int, Condition[]>> Conditions;

        public bool IsMatch(string roughPass, JToken finalToken)
        {
            if (RoughPass != roughPass)
                return false;

            JToken t = finalToken;
            int fromBottom = 0;

            foreach (var c in Conditions)
            {
                for (int i = fromBottom; i < c.Item1; ++i)
                {
                    if (t.Parent == null)
                        return false;
                    t = t.Parent;
                }

                //var index = int.Parse(t.Path.Substring(t.Path.LastIndexOf('[')).Trim('[', ']'));
                if (!c.Item2.All(x => x.IsMatch(t, 0)))
                    return false;
            }

            return true;
        }

        private PathFilter(string roughPass, List<Tuple<int, Condition[]>> conditions)
        {
            RoughPass = roughPass;
            Conditions = conditions;
        }

        private class Condition
        {
            public static Condition Parse(string src)
            {
                var match = conditionMatch.Match(src);
                if (!match.Success)
                    throw new Exception($"PathFilter condition '{src}' doesn't match regex '{conditionMatch.ToString()}'");

                var key = match.Groups["Key"].Value;
                var op = match.Groups["Op"].Value;
                var compareTo = match.Groups["CompareTo"].Value;

                Func<string, string, bool> opMethod = null;
                switch (op)
                {
                    case "=": opMethod = OpEquals; break;
                    case "!=": opMethod = OpNotEquals; break;
                    default: throw new Exception($"Invalid operation '{op}' for PathFilter operation");
                }

                return new Condition(key, opMethod, compareTo);
            }

            public bool IsMatch(JToken token, int partIndex)
            {
                string value = null;
                if (Key == "$idx")
                    value = partIndex.ToString();
                else if (token is JObject jObj)
                    value = jObj.Value<string>(Key);
                else if (token is JArray jArr)
                    value = jArr[partIndex].Value<string>();

                return Op(value, compareString);
            }

            private Condition(string key, Func<string, string, bool> op, string compareTo)
            {
                Key = key;
                compareString = compareTo;
                Op = op;
            }

            private static bool OpEquals(string a, string b) => a == b;
            private static bool OpNotEquals(string a, string b) => a != b;

            private readonly Func<string, string, bool> Op;
            private readonly string Key;
            private readonly string compareString;
            private static readonly Regex conditionMatch = new Regex(@"(?<Key>.+?)\s*(?<Op>\!?\=)(?<CompareTo>.+)", RegexOptions.Compiled);
        }
    }
}