using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Config
{
    [Config]
    public class ExtractFilterConfig
    {
        public string[] PathFilters { get; } = new[]
        {
            "events[].pages[].list[].parameters[]"
        };
    }
}
