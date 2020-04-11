using Noel.Common;
using Noel.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Setup
{
    class Program : AppBase<Program>
    {
        static void Main(string[] args)
        {
            using (var app = new Program())
                app.Run(args);
        }

        [STAThread]
        public override void AppMain()
        {
            var cfg = Environment.Config.Get<GameDirectoryConfig>();
            foreach (var season in cfg.Seasons)
            {
                if (!Directory.Exists(Path.Combine(EnvironmentDir.SeasonsDirectory, season.Root)))
                {
                    using (Logger.Context($"Downloading season {season.Number}"))
                    {
                        var fullZipfilePath = Download(season.DownloadUrl)
                            .GetAwaiter()
                            .GetResult();

                        Unzip(fullZipfilePath);
                        File.Delete(fullZipfilePath);
                    }
                }
            }
        }

        private async Task<string> Download(string downloadUrl)
        {
            var filename = Path.GetTempFileName();
            using (var wc = new WebClient())
            {
                int lastReport = 10;
                wc.DownloadProgressChanged += (s, e) => {
                    if (e.ProgressPercentage >= lastReport)
                    {
                        lastReport += 10;
                        Logger.LogLine($"{e.ProgressPercentage}% downloaded ({e.BytesReceived / 1000 / 1000}/{e.TotalBytesToReceive / 100 / 100}MB)");
                    }
                };

                await wc.DownloadFileTaskAsync(new Uri(downloadUrl), filename);
                Logger.LogLine("Done");
            }

            return filename;
        }

        private void Unzip(string fullZipfilePath)
        {
            ZipFile.ExtractToDirectory(fullZipfilePath, EnvironmentDir.SeasonsDirectory);
        }
    }
}
