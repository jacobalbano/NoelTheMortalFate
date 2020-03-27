using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Http
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
            };
        }

        public class SeasonInfo
        {
            public int Number { get; set; }
            public string[] Files { get; set; }
        }

        //[HttpGet]
        //public object File()
        //{
        //}

        //[HttpPost]
        //public void File(List<object> stuff)
        //{
        //}
    }
}
