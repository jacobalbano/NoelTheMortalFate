using Noel.Common;
using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Extract
{
    class Extract : AppBase<Extract>
    {
        static void Main(string[] args)
        {
            using (var program = new Extract())
                program.AppMain(args);
        }

        public override void AppMain(string[] args)
        {
            base.AppMain(args);
        }
    }
}
