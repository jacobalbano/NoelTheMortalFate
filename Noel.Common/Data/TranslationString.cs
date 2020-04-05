using Noel.Common.Data.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data
{
    public sealed class TranslationString
    {
        public string Address { get; set; }

        public PatchInstruction[] Instructions { get; set; } = Array.Empty<PatchInstruction>();

        public string SourceValue { get; set; }

        public string PatchValue { get; set; }
    }
}
