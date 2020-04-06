using Noel.Common;
using Noel.Common.Cache;
using Noel.Common.Config;
using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Extract
{
    class Program : AppBase<Program>
    {
        static void Main(string[] args)
        {
            using (var program = new Program())
                program.Run(args);
        }

        public override void AppMain()
        {
            using (Logger.Context("Extracting game data:"))
            {
                var config = Environment.Config.Get<ExtractFilterConfig>();
                var pathFilters = new HashSet<string>(config.PathFilters);
                foreach (var season in Environment.Seasons)
                {
                    using (Logger.Context("Season {0}", season.Number))
                    {
                        for (int i = 0; i < season.DataFilenames.Count; ++i)
                        {
                            var file = season.DataFilenames[i];
                            Logger.WriteLine("({1}/{2})\t{0}", file, i + 1, season.DataFilenames.Count);

                            var gameFile = Environment.GameFileCache.Get(season.Number, file);
                            Environment.Backup.Add(gameFile);
                            var extract = gameFile.Extract(pathFilters);
                            Environment.TranslationFileCache.Update(extract);
                        }
                    }
                }
            }

            Logger.WriteLine("Extract complete");
        }
    }
}
