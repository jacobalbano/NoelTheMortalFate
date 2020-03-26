using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noel.Common.GameData
{
    public class Season
    {
        public int Number { get; }
        public string FullFolderPath { get; }
        public string FullDataFolderPath => Path.Combine(FullFolderPath, "www");
        public string FullExecutablePath => Path.Combine(FullFolderPath, "Game.exe");

        internal Season(int number, string fullFolderPath)
        {
            Number = number;
            FullFolderPath = fullFolderPath;
        }
    }
}
