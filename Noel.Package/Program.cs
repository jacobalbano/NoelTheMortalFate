using Noel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Package
{
    class Program : AppBase<Program>
    {
        static void Main(string[] args)
        {
            using (var program = new Program())
                program.Run(args);
        }

        public override void AppMain()
        {

        }
    }
}
