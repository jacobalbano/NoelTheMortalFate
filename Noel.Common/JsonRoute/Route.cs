using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Noel.Common.JsonRoute
{
    public partial class JsonRoute
    {
        public string Source { get; private set; }

        public Point Head { get; private set; }

        public static JsonRoute Parse(string source)
        {
            var parts = source
                .Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            Point routeHead = null, point = null;
            foreach (var p in parts)
            {
                if (routeHead == null) point = routeHead = new Point();
                else point = point.Next = new Point();

                var key = path.Match(p);
                if (key.Success)
                    point.Key = key.Groups[1].Value;

                var index = indexAs.Match(p);
                if (index.Success)
                    point.CaptureIndexAs = index.Groups[1].Value;

                var captures = capture.Match(p);
                if (captures.Success)
                {
                    for (int i = 1; i < captures.Groups.Count; i++)
                    {
                        if (string.IsNullOrEmpty(captures.Groups[i].Value))
                            continue;

                        point.KeyCaptures.Add(captures.Groups[i].Value);
                    }
                }

                var preds = predicates.Match(p);
                if (preds.Success)
                {
                    for (int i = 1; i < preds.Groups.Count; i += 3)
                    {
                        var l = preds.Groups[i].Value;
                        var op = preds.Groups[i + 1].Value;
                        var r = preds.Groups[i + 2].Value;

                        if (string.IsNullOrEmpty(l) && string.IsNullOrEmpty(op) && string.IsNullOrEmpty(r))
                            continue;

                        point.Predicates.Add(Predicate.Parse(l, op, r));
                    }
                }
            }

            return new JsonRoute { Source = source, Head = routeHead };
        }

        public RouteContext AddressToContext(string address)
        {
            if (address.StartsWith("(") && address.EndsWith(")"))
                address = address.Substring(1, address.Length - 2);

            int partIndex = 0;
            var parts = address.Trim("{}".ToCharArray())
                .Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            var result = new RouteContext();
            for (var node = Head; node != null; node = node.Next)
            {
                if (node.CaptureIndexAs != null)
                {
                    var index = parts[partIndex++];
                    int.Parse(index);
                    result[node.CaptureIndexAs] = index;
                }

                foreach (var cap in node.KeyCaptures)
                    result[cap] = parts[partIndex++];
            }

            return result;
        }

        public string ContextToAddress(RouteContext context)
        {
            var points = new List<string>();
            for (var node = Head; node != null; node = node.Next)
            {
                if (context.Any(x => x.Value.Contains("トード2")))
                    ;

                if (node.CaptureIndexAs != null)
                    points.Add(context[node.CaptureIndexAs]);

                points.AddRange(node.KeyCaptures.Select(x => context[x]));
            }

            return $"({string.Join("||", points)})";
        }

        private static readonly Regex path = new Regex(@"^(\w+)", RegexOptions.Compiled);
        private static readonly Regex capture = new Regex(@"{(\w+)(?:,\s*(\w+))*}", RegexOptions.Compiled);
        private static readonly Regex indexAs = new Regex(@"\[(\w+)\]", RegexOptions.Compiled);
        private static readonly Regex predicates = new Regex(@"\((\w+)\s*([!=]{1,2})\s*(\w+)(?:\s*,\s*(\w+)\s*([!=]{1,2})\s*(\w+))*\)", RegexOptions.Compiled);

        private JsonRoute() { }
    }
}
