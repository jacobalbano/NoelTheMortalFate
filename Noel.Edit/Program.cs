using Noel.Common;
using Noel.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Noel.Edit
{
    class Program : AppBase<Program>
    {
        static void Main(string[] args)
        {
            using (var app = new Program())
                app.Run(args);
        }

        [STAThread]
        public override void AppMain()
        {
            var myServer = new SimpleHTTPServer<EditApi>("http://localhost:8823/", "resources/");
            
            Application.EnableVisualStyles();
            Application.Run(new Form1(myServer.Route));

            myServer.Stop();
        }
    }
}
