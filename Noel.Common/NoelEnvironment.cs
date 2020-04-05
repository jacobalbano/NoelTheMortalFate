using Noel.Common.Cache;
using Noel.Common.Config;
using Noel.Common.Data;
using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noel.Common
{
    public sealed class NoelEnvironment : IDisposable
    {
        public static NoelEnvironment Instance { get; private set; }

        public string RootDirectory { get; }

        public Logger Logger { get; }

        public IReadOnlyList<Season> Seasons { get; }

        public TranslationFileCache TranslationFileCache { get; }

        public GameFileCache GameFileCache { get; }

        public ConfigCache Config { get; }

        public NoelEnvironment(string environmentRoot)
        {
            RootDirectory = Path.GetFullPath(environmentRoot);
            bool firstRun = EstablishEnvironment();
            Logger = CreateFileLogger();

            using (Logger.Context("Initializing environment"))
            {
                try
                {
                    if (Instance != null)
                        throw new Exception("Environment must be initialized as a singleton");
                    Instance = this;

                    if (firstRun)
                    {
                        foreach (var dir in EnvironmentDir.Directories())
                            Directory.CreateDirectory(dir);
                    }

                    Config = new ConfigCache(firstRun);

                    var gameConfig = Config.Get<GameDirectoryConfig>();
                    Seasons = gameConfig.Seasons
                        .Select(x => new Season(x.Number, x.Root))
                        .ToList();

                    TranslationFileCache = new TranslationFileCache(this);
                    GameFileCache = new GameFileCache(this);

                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    Logger.LogLine("Abort");
                    throw new Exception("Exception occurred during environment initialization", e);
                }

                Logger.LogLine("Done");
            }
        }

        private Logger CreateFileLogger()
        {
            var filename = DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
            return new Logger(
                new ConsoleEndpoint(),
                new FileEndpoint(Path.Combine(RootDirectory, "logs", filename))
            ); ;
        }

        public void Dispose()
        {
            (Config as IDisposable).Dispose();
            Instance = null;
        }

        private bool EstablishEnvironment()
        {
            var envPath = Path.Combine(RootDirectory, "Environment.txt");
            if (File.Exists(envPath))
                return false;

            File.WriteAllText(envPath, EmbeddedFile.GetTextFile("Resources/Environment.txt"));

            return true;
        }
    }
}
