using Noel.Common;
using Noel.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Launcher
{
    class Launcher : AppBase<Launcher>
    {
        static void Main(string[] args)
        {
            using (var program = new Launcher())
                program.AppMain(args);

            Console.ReadKey();
        }

        public override void AppMain(string[] args)
        {
            base.AppMain(args);
            foreach (var chap in Environment.Seasons)
            {
                var titlePath = Path.Combine(chap.FullDataFolderPath, "img/titles1/title.png");
                if (!File.Exists(titlePath))
                    titlePath = "<not found>";

                Logger.WriteLine("{0}: {1}", chap.Number, titlePath);
            }
        }
    }
}
