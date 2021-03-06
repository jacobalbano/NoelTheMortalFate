﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Noel.Common.Logging.Endpoints
{
    public sealed class ConsoleEndpoint : ILoggerEndpoint
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose() { }
    }
}
