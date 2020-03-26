using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Noel.Common.Logging.Endpoints
{
    public sealed class FileEndpoint : ILoggerEndpoint
    {
        public FileEndpoint(string fullFilePath)
        {
            stream = File.Open(fullFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(stream);
        }

        public void WriteLine(string message)
        {
            writer.WriteLine(message);
        }

        public void Dispose()
        {
            writer.Dispose();
            stream.Dispose();
        }

        private readonly FileStream stream;
        private readonly StreamWriter writer;
    }
}
