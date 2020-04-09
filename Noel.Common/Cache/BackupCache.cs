using Newtonsoft.Json;
using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Cache
{
    public class BackupCache
    {
        public IEnumerable<string> BackupFilenames => Directory.EnumerateFiles(EnvironmentDir.BackupDirectory, "*.zip");

        public IBackupController CreateBackup()
        {
            return new BackupController(Path.Combine(EnvironmentDir.BackupDirectory, DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".zip"));
        }

        public interface IBackupController : IDisposable
        {
            void Add(GameFile gameFile);
        }

        private sealed class BackupController : IBackupController
        {
            public string BackupDestination { get; }

            public string BackupTemp { get; }

            public BackupController(string backupDestination)
            {
                BackupDestination = backupDestination;
                BackupTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(BackupTemp);
            }

#pragma warning disable CA1063 // Implement IDisposable Correctly
            void IDisposable.Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
            {
                ZipFile.CreateFromDirectory(BackupTemp, BackupDestination, CompressionLevel.Optimal, includeBaseDirectory: false);
                Directory.Delete(BackupTemp, recursive: true);
            }

            public void Add(GameFile gameFile)
            {
                var seasonDir = Path.Combine(BackupTemp, gameFile.SeasonNum.ToString());
                Directory.CreateDirectory(seasonDir);
                File.WriteAllText(Path.Combine(seasonDir, gameFile.Filename + ".json"), gameFile.JsonData.ToString(Formatting.None), encoding);
            }

            private static readonly UTF8Encoding encoding = new UTF8Encoding(false);
        }
    }
}
