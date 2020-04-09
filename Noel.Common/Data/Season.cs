using Noel.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Noel.Common.Data
{
    public class Season
    {
        public int Number { get; }
        public string FullFolderPath { get; }
        public string FullResourceFolderPath => Path.Combine(FullFolderPath, "www");
        public string FullJsonFolderPath => Path.Combine(FullResourceFolderPath, "data");
        public string FullExecutablePath => Path.Combine(FullFolderPath, "Game.exe");
        public string FullWorkingFolderPath => Path.Combine(EnvironmentDir.WorkingDirectory, Number.ToString());

        public IReadOnlyList<string> DataFilenames => dataFilenames ?? (dataFilenames = GetDataFilenames());
        public IReadOnlyList<string> TranslationFilenames => translationFilenames ?? (translationFilenames = GetTranslationFilenames());

        internal Season(int number, string relativeFolderPath)
        {
            Number = number;
            FullFolderPath = Path.Combine(EnvironmentDir.SeasonsDirectory, relativeFolderPath);
        }

        private List<string> GetDataFilenames()
        {
            return Directory.EnumerateFiles(FullJsonFolderPath, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        private List<string> GetTranslationFilenames()
        {
            Directory.CreateDirectory(FullWorkingFolderPath);
            return Directory.EnumerateFiles(FullWorkingFolderPath, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        private List<string> dataFilenames, translationFilenames;
    }
}
