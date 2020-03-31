using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.JsonRoute
{
    public class RouteContext : Dictionary<string, string>
    {
        [DebuggerStepThrough]
        public RouteContext Clone()
        {
            var result = new RouteContext();
            foreach (var pair in this)
                result[pair.Key] = pair.Value;

            return result;
        }
    }
}
