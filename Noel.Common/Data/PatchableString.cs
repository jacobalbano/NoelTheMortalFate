using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data
{
    public sealed class PatchableString
    {
        public string Address { get; set; }

        public PatchInstruction[] Instructions { get; set; }

        public string Japanese { get; set; }

        public string English { get; set; }
    }
}
