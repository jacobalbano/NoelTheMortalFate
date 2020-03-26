using System;
using System.Collections.Generic;
using System.Text;

namespace Noel.Common.Logging
{
    public interface ILoggerEndpoint : IDisposable
    {
        void WriteLine(string message);
    }
}
