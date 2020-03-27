using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noel.Common
{
    public abstract class AppBase<TApp> : IDisposable where TApp : AppBase<TApp>
    {
        public static NoelEnvironment Environment { get; private set; }

        public static Logger Logger { get; private set;  }

        public virtual void AppMain(string[] args)
        {
            if (Environment != null)
                throw new Exception("AppBase<TApp> must be initialized as a singleton");

            if (args.Any())
                System.Environment.CurrentDirectory = args[0];

            Environment = new NoelEnvironment(out var logger);
            Logger = logger;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            Environment.Dispose();
            Logger.Dispose();

            Environment = null;
            Logger = null;
            disposed = true;
        }

        private bool disposed = false;
    }
}
