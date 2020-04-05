using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.Config
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ConfigAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class LiveConfigAttribute : ConfigAttribute
    {
    }
}
