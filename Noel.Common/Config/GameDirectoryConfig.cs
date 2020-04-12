using System;
using System.Collections.Generic;
using System.Text;

namespace Noel.Common.Config
{
    [Config]
    public class GameDirectoryConfig
    {
        public SeasonConfig[] Seasons { get; set; } = new[]
        {
            new SeasonConfig { Number = 1, Root = @"noel_1d\noel_s1\", TitleImagePath = @"img\titles1\title.png", DownloadUrl = "http://www.gamemaga.jp/download_play/noel_1d.zip" },
            new SeasonConfig { Number = 2, Root = @"noel_2c\noel_s2a\", TitleImagePath = @"img\titles1\title2.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_2c.zip" },
            new SeasonConfig { Number = 3, Root = @"noel_3a\noel_s3\", TitleImagePath = @"img\titles1\title3.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_3a.zip" },
            new SeasonConfig { Number = 4, Root = @"noel_4a\noel_s4\", TitleImagePath = @"img\titles1\title4.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_4a.zip" },
            new SeasonConfig { Number = 5, Root = @"noel_5b\noel_s5\", TitleImagePath = @"img\titles1\title5.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_5b.zip" },
            new SeasonConfig { Number = 6, Root = @"noel_6\noel_6\", TitleImagePath = @"img\titles1\title6.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_6.zip" },
            new SeasonConfig { Number = 7, Root = @"noel_7\noel_7\", TitleImagePath = @"img\titles1\title7.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_7.zip" },
            new SeasonConfig { Number = 8, Root = @"noel_8\noel8\", TitleImagePath = @"img\titles1\title8.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_8.zip" },
            new SeasonConfig { Number = 9, Root = @"noel_9b\noel_s9\", TitleImagePath = @"img\titles1\title9.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_9b.zip" },
            new SeasonConfig { Number = 10, Root = @"noel_10c\noel_s10\", TitleImagePath = @"img\titles1\title10.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_10c.zip" },
            new SeasonConfig { Number = 11, Root = @"noel_11a\", TitleImagePath = @"img\titles1\title11.png", DownloadUrl = "https://www.gamemaga.jp/download_play/noel_11a.zip" },
        };
    }

    public class SeasonConfig
    {
        public int Number { get; set; }

        public string Root { get; set; }

        public string TitleImagePath { get; set; }

        public string DownloadUrl { get; set; }
    }
}
