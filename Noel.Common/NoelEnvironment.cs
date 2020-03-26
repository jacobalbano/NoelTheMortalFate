using Noel.Common.Config;
using Noel.Common.GameData;
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

        public IReadOnlyList<Season> Seasons { get; }

        public NoelEnvironment(out Logger logger)
        {
            bool firstRun = EstablishEnvironment();
            logger = CreateFileLogger();

            using (logger.Context("Initializing environment"))
            {
                try
                {
                    if (Instance != null)
                        throw new Exception("Environment must be initialized as a singleton");

                    if (firstRun)
                        ConfigManager.Establish();
                    else
                        ConfigManager.Load();
                    
                    //Validate();
                }
                catch (Exception e)
                {
                    logger.LogException(e);
                }

                logger.LogLine("done");
            }

            Instance = this;
        }

        private Logger CreateFileLogger()
        {
            var filename = DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
            return new Logger(
                new ConsoleEndpoint(),
                new FileEndpoint(Path.Combine("logs", filename))
            ); ;
        }

        private void Validate()
        {
            foreach (var chapter in Seasons)
            {
                try
                {
                    if (!Directory.Exists(chapter.FullFolderPath))
                        throw new Exception("Chapter root does not exist");

                    if (!File.Exists(chapter.FullExecutablePath))
                        throw new Exception("Game.exe does not exist");

                    if (!Directory.Exists(chapter.FullDataFolderPath))
                        throw new Exception("Data root does not exist");
                }
                catch (Exception e)
                {
                    throw new Exception($"Error encountered while validating folder structure for chapter {chapter.Number}", e);
                }
            }
        }

        public void Dispose()
        {
            Instance = null;
        }

        private static bool EstablishEnvironment()
        {
            if (File.Exists("Environment.txt"))
                return false;

            File.WriteAllText("Environment.txt", Embeds.GetTextFile("Resources/Environment.txt"));

            foreach (var dir in Embeds.GetTextLines("EnvFolders.txt"))
                Directory.CreateDirectory(dir);

            return true;
        }
    }
}
