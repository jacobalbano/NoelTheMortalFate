using Noel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Patch
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
            using (Logger.Context("Patching game data:"))
            {
                int totalStrings = 0;
                foreach (var season in Environment.Seasons)
                {
                    using (Logger.Context($"Season {season.Number}"))
                    {
                        int seasonTotal = 0;
                        foreach (var file in season.TranslationFilenames.Progress())
                        {
                            var transFile = Environment.TranslationFileCache.Get(season.Number, file);
                            var gameFile = Environment.GameFileCache.Get(season.Number, file);
                            var linesChanged = gameFile.Patch(transFile);

                            if (linesChanged > 0)
                            {
                                seasonTotal += linesChanged;
                                Environment.GameFileCache.Update(gameFile);
                                Logger.LogLine($"{linesChanged} strings patched from {file}\t({file.Number}/{file.Total} files processed)");
                            }
                        }

                        totalStrings += seasonTotal;
                        Logger.LogLine($"{seasonTotal} strings patched into season");
                    }
                }

                Logger.LogLine($"{totalStrings} strings patched across all seasons");
                Logger.LogLine("Done");
            }
        }
    }
}
