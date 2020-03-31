using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Noel.Common.JsonRoute
{
    public class ExtractVisitor
    {
        public class Data
        {
            public string Path { get; set; }
            public JArray Content { get; set; }
        }

        public IReadOnlyList<Data> Storage => storage;

        public void Visit(JToken rootObject, JsonRoute.Point node)
        {
            if (node == null)
                return;

            Visit(rootObject, node, new RouteContext());
        }

        protected void Store(JToken token)
        {
            storage.Add(new Data { Content = (JArray) token, Path = token.Path});
        }

        private void Visit(JToken rootObject, JsonRoute.Point node, RouteContext ctx)
        {
            if (node == null)   //  reached the end
            {
                if (rootObject.Type != JTokenType.Array)
                    throw new Exception($"Route lead to an object of type {rootObject.Type}");

                Store(rootObject);
                return;
            }

            if (node.CaptureIndexAs == null)
                VisitKeyedNode(rootObject, node, ctx.Clone());
            else
                VisitIteration(rootObject, node, ctx.Clone());
        }

        private void VisitIteration(JToken rootObject, JsonRoute.Point node, RouteContext ctx)
        {
            int index = 0;
            var iterant = rootObject;

            if (node.Key != null)
            {
                if (int.TryParse(node.Key, out var i))
                    iterant = rootObject.Skip(i).First();
                else
                    iterant = rootObject[node.Key];
            }

            foreach (var child in iterant.Children())
            {
                var clone = ctx.Clone();
                clone[node.CaptureIndexAs] = index.ToString();
                ++index;

                if (node.KeyCaptures.Any() && !child.HasValues) continue;

                foreach (var cap in node.KeyCaptures)
                    clone[cap] = child[cap].ToString();

                if (!node.Predicates.All(pred =>
                {
                    if (!clone.TryGetValue(pred.Key, out var value))
                        value = child[pred.Key].ToString();

                    return pred.IsMatch(value);
                })) continue;

                Visit(child, node.Next, clone);
            }
        }

        private void VisitKeyedNode(JToken rootObject, JsonRoute.Point node, RouteContext ctx)
        {
            object key = node.Key;
            if (int.TryParse(node.Key, out var i))
                key = i;

            var clone = ctx.Clone();
            foreach (var cap in node.KeyCaptures)
            {
                object capKey = cap;
                if (int.TryParse(cap, out var j))
                    capKey = j;

                clone[cap] = rootObject[capKey].ToString();
            }

            Visit(rootObject[key], node.Next, clone);
        }

        private List<Data> storage = new List<Data>();
    }
}
