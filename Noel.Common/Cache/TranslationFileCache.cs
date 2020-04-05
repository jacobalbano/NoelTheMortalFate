using Newtonsoft.Json;
using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            if (file.Strings.Length > 0)
                File.WriteAllText(GetPath(file.SeasonNum, file.Filename), JsonConvert.SerializeObject(file, Formatting.Indented), encoding);

            cache[MakeCacheKey(file.SeasonNum, file.Filename)] = file;
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
        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);
        private readonly NoelEnvironment Environment;
    }
}
