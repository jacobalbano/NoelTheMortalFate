using Newtonsoft.Json;
using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Noel.Common.Cache
{
    public class TranslationFileCache
    {
        public TranslationFileCache(NoelEnvironment env)
        {
            Environment = env;
        }

        public TranslationFile Get(int seasonNum, string filename)
        {
            var key = MakeCacheKey(seasonNum, filename);
            if (cache.TryGetValue(key, out var result))
                return result;

            try
            {
                var contents = File.ReadAllText(GetPath(seasonNum, filename), encoding);
                cache[key] = result = JsonConvert.DeserializeObject<TranslationFile>(contents);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't load translation file: Season {seasonNum}, file '{filename}'", e);
            }
        }

        public void Update(TranslationFile file)
        {
            var cacheKey = MakeCacheKey(file.SeasonNum, file.Filename);

            TranslationFile existingFile = null;
            try { existingFile = Get(file.SeasonNum, file.Filename); } catch { }

            if (existingFile == null)
                cache[cacheKey] = file;
            else
            {
                //  merging
                var allStrings = existingFile.Strings.ToDictionary(x => x.Address);

                foreach (var newStr in file.Strings)
                {
                    if (!allStrings.TryGetValue(newStr.Address, out var oldStr))
                        allStrings[newStr.Address] = newStr;    //  new string, maybe updated filters
                    else if (!string.IsNullOrEmpty(newStr.PatchValue))
                        allStrings[newStr.Address] = newStr; //  updating translation
                }

                file.Strings = allStrings
                    .Select(x => x.Value)
                    .OrderBy(x => x.Address, comparer)
                    .ToArray();
            }

            if (file.Strings.Length > 0)
                File.WriteAllText(GetPath(file.SeasonNum, file.Filename), JsonConvert.SerializeObject(file, Formatting.Indented), encoding);

            cache[cacheKey] = file;
        }

        private string GetPath(int seasonNum, string filename)
        {
            var season = Environment.Seasons.Single(x => x.Number == seasonNum);
            var dataFolder = season.FullWorkingFolderPath;
            return Path.Combine(dataFolder, filename + ".json");
        }

        private static string MakeCacheKey(int seasonNum, string filename)
        {
            return $"#{seasonNum}|{filename}";
        }

        private readonly Dictionary<string, TranslationFile> cache = new Dictionary<string, TranslationFile>();
        private readonly IComparer<string> comparer = new NaturalSortComparer();
        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);
        private readonly NoelEnvironment Environment;

        private sealed class NaturalSortComparer : IComparer<string>
        {
            int IComparer<string>.Compare(string x, string y)
            {
                if (x == y) return 0;
                if (!table.TryGetValue(x, out var x1))
                    table[x] = x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");

                if (!table.TryGetValue(y, out var y1))
                    table[y] = y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");

                for (int i = 0; i < x1.Length && i < y1.Length; i++)
                {
                    if (x1[i] != y1[i])
                        return PartCompare(x1[i], y1[i]);
                }

                if (y1.Length > x1.Length) return 1;
                else if (x1.Length > y1.Length) return -1;
                else return 0;
            }

            private static int PartCompare(string left, string right)
            {
                if (!int.TryParse(left, out var x)) return left.CompareTo(right);
                if (!int.TryParse(right, out var y)) return left.CompareTo(right);
                return x.CompareTo(y);
            }

            private readonly Dictionary<string, string[]> table = new Dictionary<string, string[]>();
        }
    }
}
