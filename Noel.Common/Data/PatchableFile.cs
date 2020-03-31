using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data
{
    public sealed class PatchableFile
    {
        public int SeasonNum { get; set; }

        public string Filename {get; set; }

        public PatchableString[] Strings { get; set; }
    }
}
