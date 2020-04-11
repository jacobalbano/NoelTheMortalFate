using Noel.Common.Logging.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        static AppBase()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("Trying to load from bin for {0}", args.Name);

            // Ignore missing resources
            if (args.Name.Contains(".resources"))
                return null;

            // check for assemblies already loaded
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            // Try to load by filename - split out the filename of the full assembly name
            // and append the base path of the original assembly (ie. look in the same dir)
            string filename = args.Name.Split(',')[0] + ".dll".ToLower();

            string asmFile = Path.Combine(EnvironmentDir.RootDirectory, "bin", filename);

            try { return Assembly.LoadFrom(asmFile); }
            catch (Exception e) { Logger.LogException(e); return null; }
        }
    }
}
