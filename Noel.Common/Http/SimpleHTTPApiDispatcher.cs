using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;

namespace Noel.Common.Http
{
    class SimpleHTTPApiDispatcher
    {
        public SimpleHTTPApiDispatcher(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            var expr = Expression.Convert(Expression.New(ctor), typeof(object));
            Factory = Expression.Lambda<Func<object>>(expr).Compile();

            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                string httpMethod = null;
                if (method.IsDefined(typeof(HttpGetAttribute)))
                    httpMethod = "GET";
                else if (method.IsDefined(typeof(HttpPostAttribute)))
                    httpMethod = "POST";
                else throw new Exception($"Bad method {method.Name} on API");

                cache[MakeCacheKey(httpMethod, method.Name)] = method;
            }
        }

        private string MakeCacheKey(string method, string name)
        {
            return $"{method}:{name.ToUpper()}";
        }

        public string CallMethod(string methodName, string methodType, string rawArgs)
        {
            var target = Factory();
            if (cache.TryGetValue(MakeCacheKey(methodType, methodName), out var method))
            {
                switch (methodType)
                {
                    case "GET":
                        var args = rawArgs?.Split('&')
                            .Select(x => x.Split('='))
                            .ToDictionary(x => x[0], x => HttpUtility.UrlDecode(x[1]));

                        var typeArgs = method.GetParameters()
                            .Select((p, i) => Convert.ChangeType(args[p.Name], p.ParameterType))
                            .ToArray();

                        var result = method.Invoke(target, typeArgs);
                        return JsonConvert.SerializeObject(result, camelCase);
                    case "POST":
                        var param = method.GetParameters().First().ParameterType;
                        var obj = JsonConvert.DeserializeObject(rawArgs, param);
                        method.Invoke(target, new[] { obj });
                        return null;
                }
            }

            throw new Exception($"Invalid method {methodType} '{methodName}'");
        }

        private readonly Func<object> Factory;
        private readonly Dictionary<string, MethodInfo> cache = new Dictionary<string, MethodInfo>();
        private static readonly JsonSerializerSettings camelCase = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
