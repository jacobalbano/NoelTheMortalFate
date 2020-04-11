using Newtonsoft.Json.Linq;
using Noel.Common.Config;
using Noel.Common.Data;
using Noel.Common.Data.Instructions;
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

        public TranslationFile Extract(PathFilter[] pathFilters)
        {
            return new TranslationFile
            {
                Filename = Filename,
                SeasonNum = SeasonNum,
                Strings = Explore(JsonData, pathFilters).ToArray()
            };
        }

        private static IEnumerable<TranslationString> Explore(JToken obj, PathFilter[] pathFilters)
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
                if (!string.IsNullOrWhiteSpace(strVal))
                {
                    var roughPass = new string(obj.Path.Where(x => !char.IsNumber(x)).ToArray());
                    if (pathFilters.Any(x => x.IsMatch(roughPass, obj)))
                        yield return new TranslationString { Address = obj.Path, SourceValue = strVal };
                }
            }
        }

        public int Patch(TranslationFile data)
        {
            var allStrings = data.Strings.ToDictionary(x => x.Address);

            int patched = 0;
            foreach (var str in data.Strings)
            {
                bool skip = false;
                foreach (var inst in str.Instructions)
                {
                    if (inst is SkipInstruction)
                        skip = true;

                    inst.Apply(allStrings, str);
                }

                if (skip)
                {

                    continue;
                }

                if (string.IsNullOrEmpty(str.PatchValue))
                    continue;

                var target = JsonData.SelectToken(str.Address);
                target.Replace(str.PatchValue);
                patched++;
            }

            return patched;
        }
    }
}
