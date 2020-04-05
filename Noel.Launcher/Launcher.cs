﻿using Noel.Common;
using Noel.Common.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                program.Run(args);
        }

        public override void AppMain()
        {
            var xnaCfg = Environment.Config.Get<XnaAppConfig>();
            using (var app = new XnaApp(xnaCfg))
            {
                app.Run();
                if (app.ChosenGamePath != null)
                    Process.Start(app.ChosenGamePath);
            }
        }
    }
}
