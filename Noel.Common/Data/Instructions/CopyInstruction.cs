using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data.Instructions
{
    public sealed class CopyInstruction : PatchInstruction
    {
        public string LineReference{ get; set; }

        public override void Apply(Dictionary<string, TranslationString> allStrings, TranslationString currentString)
        {
            if (!allStrings.TryGetValue(LineReference, out var fromStr))
                throw new IndexOutOfRangeException($"Attempted to copy from a string that was out of range (reference '{LineReference}')");

            currentString.PatchValue = fromStr.PatchValue;
        }
    }
}
