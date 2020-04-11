using Noel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Noel.DevInstall
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
            string solutionDir = null;
            using (Logger.Context("Locating solution"))
            {
                var asmLocation = Assembly.GetEntryAssembly().Location;
                solutionDir = new FileInfo(asmLocation).Directory.Parent.Parent.Parent.FullName;
                if (!File.Exists(Path.Combine(solutionDir, "NoelTheMortalFate.sln")))
                    throw new Exception("Couldn't ascertain location of NoelTheMortalFate solution");

                Logger.LogLine("Done");
            }

            var copyPaths = new List<Tuple<string, string>>();
            using (Logger.Context("Exploring for binaries"))
            {
                var projects = new[] { "Noel.Edit", "Noel.Extract", "Noel.Launcher", "Noel.Patch", "Noel.Setup" };
                foreach (var p in projects)
                {
                    using (Logger.Context(p))
                    {
                        var projectBin = Path.Combine(solutionDir, p, "bin", "debug");
                        foreach (var dll in Directory.EnumerateFiles(projectBin, "*.dll").Where(x => !x.Contains("Noel.Common")))
                        {
                            var file = new FileInfo(dll);
                            copyPaths.Add(Tuple.Create(file.FullName, Path.Combine("bin", file.Name)));
                            Logger.LogLine(file.Name);
                        }

                        var exeName = p + ".exe";
                        var exePath = Path.Combine(projectBin, exeName);
                        if (!File.Exists(exePath))
                            throw new Exception($"Missing file {exeName}");
                        
                        copyPaths.Add(Tuple.Create(exePath, exeName));
                        Logger.LogLine(exeName);
                    }
                }

                copyPaths.Add(Tuple.Create(Path.Combine(solutionDir, "Noel.Common", "bin", "debug", "Noel.Common.dll"), "Noel.Common.dll"));

                Logger.LogLine("Done");
            }

            using (Logger.Context("Copying binaries"))
            {
                Directory.CreateDirectory(Path.Combine(EnvironmentDir.RootDirectory, "bin"));
                foreach (var paths in copyPaths)
                {
                    File.Copy(paths.Item1, Path.Combine(EnvironmentDir.RootDirectory, paths.Item2), overwrite: true);
                    Logger.LogLine(paths.Item1);
                }

                Logger.LogLine("Done");
            }
        }
    }
}
