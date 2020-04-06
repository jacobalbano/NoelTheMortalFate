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

        public Backup Backup { get; }

        public ConfigCache Config { get; }

        public NoelEnvironment(string environmentRoot)
        {
            if (Instance != null)
                throw new Exception("Environment must be initialized as a singleton");
            Instance = this;

            RootDirectory = Path.GetFullPath(environmentRoot);
            bool firstRun = EstablishEnvironment();
            if (firstRun)
            {
                foreach (var dir in EnvironmentDir.Directories())
                    Directory.CreateDirectory(dir);
            }

            Logger = CreateFileLogger();
            using (Logger.Context("Initializing environment"))
            {
                try
                {
                    Config = new ConfigCache(firstRun);

                    var gameConfig = Config.Get<GameDirectoryConfig>();
                    Seasons = gameConfig.Seasons
                        .Where(x => Directory.Exists(Path.Combine(EnvironmentDir.SeasonsDirectory, x.Root)))
                        .Select(x => new Season(x.Number, x.Root))
                        .ToList();

                    foreach (var season in Seasons)
                    {
                        Directory.CreateDirectory(season.FullWorkingFolderPath);
                        Directory.CreateDirectory(season.FullBackupFolderPath);
                    }

                    TranslationFileCache = new TranslationFileCache(this);
                    GameFileCache = new GameFileCache(this);
                    Backup = new Backup();

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
