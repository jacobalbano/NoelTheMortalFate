using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Launcher.FNA
{
    public static class Approach
    {
        public static void TowardsWithDecay(ref float target, float to, float amount = 0.1f)
        {
            target += (to - target) * amount;
        }
    }
}
