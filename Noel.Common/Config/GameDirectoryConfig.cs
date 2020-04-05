using System;
using System.Collections.Generic;
using System.Text;

namespace Noel.Common.Config
{
    [Config]
    public class GameDirectoryConfig
    {
        public SeasonConfig[] Seasons { get; } = new[]
        {
            new SeasonConfig { Number = 1, Root = @"chapters\noel_1d\noel_s1\", TitleImagePath = @"img\titles1\title.png" },
            new SeasonConfig { Number = 2, Root = @"chapters\noel_2c\noel_s2a\", TitleImagePath = @"img\titles1\title2.png" },
            new SeasonConfig { Number = 3, Root = @"chapters\noel_3a\noel_s3\", TitleImagePath = @"img\titles1\title3.png" },
            new SeasonConfig { Number = 4, Root = @"chapters\noel_4a\noel_s4\", TitleImagePath = @"img\titles1\title4.png" },
            new SeasonConfig { Number = 5, Root = @"chapters\noel_5b\noel_s5\", TitleImagePath = @"img\titles1\title5.png" },
            new SeasonConfig { Number = 6, Root = @"chapters\noel_6\noel_s6\", TitleImagePath = @"img\titles1\title6.png" },
            new SeasonConfig { Number = 7, Root = @"chapters\noel_7\noel_7\", TitleImagePath = @"img\titles1\title7.png" },
            new SeasonConfig { Number = 8, Root = @"chapters\noel_8\noel8\", TitleImagePath = @"img\titles1\title8.png" },
            new SeasonConfig { Number = 9, Root = @"chapters\noel_9b\noel_s9\", TitleImagePath = @"img\titles1\title9.png" },
            new SeasonConfig { Number = 10, Root = @"chapters\noel_10c\noel_s10\", TitleImagePath = @"img\titles1\title10.png" },
            new SeasonConfig { Number = 11, Root = @"chapters\noel_11a\", TitleImagePath = @"img\titles1\title11.png" },
        };
    }

    public class SeasonConfig
    {
        public int Number { get; set; }

        public string Root { get; set; }

        public string TitleImagePath { get; set; }
    }
}
