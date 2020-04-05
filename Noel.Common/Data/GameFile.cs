using Newtonsoft.Json.Linq;
using Noel.Common.Config;
using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data
{
    public sealed class GameFile
    {
        public int SeasonNum { get; }
        
        public string Filename { get; }

        public JToken JsonData { get; }

        internal GameFile(int seasonNum, string filename, JToken jsonData)
        {
            SeasonNum = seasonNum;
            Filename = filename;
            JsonData = jsonData;
        }

        public TranslationFile Extract(HashSet<string> pathFilters)
        {
            return new TranslationFile
            {
                Filename = Filename,
                SeasonNum = SeasonNum,
                Strings = Explore(JsonData, pathFilters).ToArray()
            };
        }

        private static IEnumerable<TranslationString> Explore(JToken obj, HashSet<string> pathFilters)
        {
            if (obj.Type == JTokenType.Object)
            {
                foreach (var str in ((JObject)obj).Properties().SelectMany(x => Explore(x, pathFilters)))
                    yield return str;
            }
            else if (obj.Type == JTokenType.Array)
            {
                foreach (var str in ((JArray)obj).Children().SelectMany(x => Explore(x, pathFilters)))
                    yield return str;
            }
            else if (obj.Type == JTokenType.Property)
            {
                var prop = (JProperty)obj;
                var val = prop.Value;
                switch (val.Type)
                {
                    case JTokenType.Object:
                    case JTokenType.Array:
                    case JTokenType.String:
                        foreach (var str in Explore(val, pathFilters))
                            yield return str;
                        break;
                }
            }
            else if (obj.Type == JTokenType.String)
            {
                var strVal = obj.Value<string>();
                if (!string.IsNullOrEmpty(strVal))
                {
                    var path = new string(obj.Path.Where(x => !char.IsNumber(x)).ToArray());
                    if (pathFilters.Contains(path))
                        yield return new TranslationString { Address = obj.Path, SourceValue = strVal };
                }
            }
        }

        public void Patch(TranslationFile data)
        {
            for (int i = 0; i < data.Strings.Length; ++i)
            {
                var str = data.Strings[i];
                foreach (var inst in str.Instructions)
                    inst.Apply(data, str, i);

                if (string.IsNullOrEmpty(str.PatchValue))
                    continue;

                var target = JsonData.SelectToken(str.Address);
                target.Replace(str.PatchValue);
            }
        }
    }
}
