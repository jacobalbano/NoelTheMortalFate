using Noel.Common;
using Noel.Common.Cache;
using Noel.Common.Config;
using Noel.Common.Data;
using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
                int totalStrings = 0;
                using (var backup = Environment.BackupCache.CreateBackup())
                {
                    var config = Environment.Config.Get<ExtractFilterConfig>();
                    var pathFilters = config.PathFilters.Select(PathFilter.Parse).ToArray();
                    foreach (var season in Environment.Seasons)
                    {
                        using (Logger.Context($"Season {season.Number}"))
                        {
                            int seasonTotal = 0;
                            foreach (var item in season.DataFilenames.Progress())
                            {
                                var gameFile = Environment.GameFileCache.Get(season.Number, item);
                                var extract = gameFile.Extract(pathFilters);
                                if (extract.Strings.Any())
                                {
                                    seasonTotal += extract.Strings.Length;
                                    Environment.TranslationFileCache.Update(extract, isExtract: true);
                                    backup.Add(gameFile);

                                    Logger.LogLine($"{extract.Strings.Length} strings extracted from {item}\t({item.Number}/{item.Total} files processed)");
                                }
                            }

                            totalStrings += seasonTotal;
                            Logger.LogLine($"{seasonTotal} strings extracted from season");
                        }
                    }

                    Logger.LogLine($"{totalStrings} strings extracted across all seasons");
                    Logger.LogLine("Saving backup file...");
                }

                Logger.LogLine("Done");
            }
        }
    }
}
