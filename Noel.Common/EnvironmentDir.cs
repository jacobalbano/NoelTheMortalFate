using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common
{
    public static class EnvironmentDir
    {
        public static string RootDirectory => NoelEnvironment.Instance.RootDirectory;

        public static string ConfigDirectory => Path.Combine(RootDirectory, "cfg");

        public static string WorkingDirectory => Path.Combine(RootDirectory, "hyworking");
        
        public static string BackupDirectory => Path.Combine(RootDirectory, "backup");
        
        public static string LogDirectory => Path.Combine(RootDirectory, "logs");

        public static IEnumerable<string> Directories()
        {
            yield return ConfigDirectory;
            yield return WorkingDirectory;
            yield return BackupDirectory;
            yield return LogDirectory;
        }
    }
}
