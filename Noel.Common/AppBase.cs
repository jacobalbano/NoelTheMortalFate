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

        public void Run(string[] args)
        {
            if (Environment != null)
                throw new Exception("AppBase<TApp> must be initialized as a singleton");

            Environment = new NoelEnvironment(args.FirstOrDefault() ?? ".");
            Logger = Environment.Logger;

            try
            {
                AppMain();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }
        }

        public abstract void AppMain();

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
