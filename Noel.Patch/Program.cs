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
            foreach (var season in Environment.Seasons)
            {
                foreach (var file in season.TranslationFilenames)
                {
                    var transFile = Environment.TranslationFileCache.Get(season.Number, file);
                    var gameFile = Environment.GameFileCache.Get(season.Number, file);
                    gameFile.Patch(transFile);
                    Environment.GameFileCache.Update(gameFile);
                }
            }
        }
    }
}
