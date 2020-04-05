using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Config
{
    [LiveConfig]
    public class XnaAppConfig
    {
        public int LastSelectedSeason { get; set; } = 1;
        public int WindowResolutionX { get; set; } = 1280;
        public int WindowResolutionY { get; set; } = 720;

        public int InternalResolutionX { get; set; } = 1280;
        public int InternalResolutionY { get; set; } = 720;
    }
}
