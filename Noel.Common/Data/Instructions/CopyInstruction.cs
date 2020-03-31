using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Data.Instructions
{
    public sealed class CopyInstruction : PatchInstruction
    {
        public int LineNumber { get; set; }

        public override void Apply(PatchableFile currentFile, PatchableString currentString, int currentLine)
        {
            if (LineNumber >= currentFile.Strings.Length)
                throw new IndexOutOfRangeException($"Attempted to copy from a string that was out of range (index {LineNumber})");

            var fromString = currentFile.Strings[LineNumber];
            currentString.English = fromString.English;
        }
    }
}
