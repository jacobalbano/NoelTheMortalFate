using Noel.Common;
using Noel.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Edit
{
    class Program : AppBase<Program>
    {
        static void Main(string[] args)
        {
            using (var app = new Program())
                app.AppMain(args);
        }

        public override void AppMain(string[] args)
        {
            base.AppMain(args);

            var myServer = new SimpleHTTPServer<EditApi>("http://localhost:8823/", "resources/");
            Console.ReadKey();
            myServer.Stop();
        }
    }
}
