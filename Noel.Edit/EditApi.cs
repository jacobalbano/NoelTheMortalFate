using Noel.Common.Data;
using Noel.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Edit
{
    class EditApi
    {
        [HttpGet]
        public SeasonInfo[] Seasons()
        {
            return new[]
            {
                new SeasonInfo { Number = 1, Files = new[] { "Map004", "Map005", "Map006" } },
                new SeasonInfo { Number = 2, Files = new[] { "Map001", "Map002", "Map003", "Map004" } },
                new SeasonInfo { Number = 3, Files = new[] { "Map003", "CommonEvents" } },
                new SeasonInfo { Number = 4, Files = new[] { "Map004", "Map005", "Map006" } },
                new SeasonInfo { Number = 5, Files = new[] { "Map001", "Map002", "Map003", "Map004" } },
                new SeasonInfo { Number = 6, Files = new[] { "Map003", "CommonEvents" } },
                new SeasonInfo { Number = 7, Files = new[] { "Map004", "Map005", "Map006" } },
                new SeasonInfo { Number = 8, Files = new[] { "Map001", "Map002", "Map003", "Map004" } },
                new SeasonInfo { Number = 9, Files = new[] { "Map003", "CommonEvents" } },
                new SeasonInfo { Number = 10, Files = new[] { "Map004", "Map005", "Map006" } },
                new SeasonInfo { Number = 11, Files = new[] { "Map001", "Map002", "Map003", "Map004" } },
                new SeasonInfo { Number = 12, Files = new[] { "Map003", "CommonEvents" } },
            };
        }

        [HttpGet]
        public PatchableFile Strings(int seasonNum, string filename)
        {
            var strings = new[]
            {
                new PatchableString { Address = "1/2/3", Instructions = new PatchInstruction[0], Japanese = "少し時間をつぶしたほうがいいかもしれない", English = "" },
				new PatchableString { Address = "1/2/4", Instructions = new PatchInstruction[0], Japanese = "ジリアン君はたしか市街地の出身だったかな？", English = "" },
				new PatchableString { Address = "2/1/3", Instructions = new PatchInstruction[0], Japanese = "それは演奏そのものとは関係のないことだ。", English = "" },
				new PatchableString { Address = "5/4/4", Instructions = new PatchInstruction[0], Japanese = "もっと自分のピアノに自信を持つといい。", English = "" },
				new PatchableString { Address = "1/2/3", Instructions = new PatchInstruction[0], Japanese = "少し時間をつぶしたほうがいいかもしれない", English = "" },
            };

            return new PatchableFile { SeasonNum = seasonNum, Filename = filename, Strings = strings };
        }

        [HttpPost]
        public void Strings(PatchableFile file)
        {
        }
    }
}
