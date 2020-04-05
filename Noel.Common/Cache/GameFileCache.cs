using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Cache
{
    public class GameFileCache
    {
        public GameFileCache(NoelEnvironment env)
        {
            Environment = env;
        }

        public GameFile Get(int seasonNum, string filename)
        {
            var key = MakeCacheKey(seasonNum, filename);
            if (cache.TryGetValue(key, out var result))
                return result;

            try
            {
                var contents = File.ReadAllText(GetPath(seasonNum, filename), encoding);
                cache[key] = result = new GameFile(seasonNum, filename, JToken.Parse(contents));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't load game data file: Season {seasonNum}, file '{filename}'", e);
            }
        }

        public void Update(GameFile file)
        {
            var path = GetPath(file.SeasonNum, file.Filename);
            File.WriteAllText(path, file.JsonData.ToString(Formatting.None));
            cache[MakeCacheKey(file.SeasonNum, file.Filename)] = file;
        }

        private static string MakeCacheKey(int seasonNum, string filename)
        {
            return $"#{seasonNum}|{filename}";
        }

        private string GetPath(int seasonNum, string filename)
        {
            var season = Environment.Seasons.Single(x => x.Number == seasonNum);
            var dataFolder = season.FullJsonFolderPath;
            return Path.Combine(dataFolder, filename + ".json");
        }

        private readonly Dictionary<string, GameFile> cache = new Dictionary<string, GameFile>();
        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);
        private readonly NoelEnvironment Environment;
    }
}
