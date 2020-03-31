using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.JsonRoute
{
    public partial class JsonRoute
    {
        public class Point
        {
            public Point Next { get; set; }

            //  can be name or index
            public string Key { get; set; }

            //  property names
            public List<string> KeyCaptures { get; set; }

            public string CaptureIndexAs { get; set; }

            public List<Predicate> Predicates { get; set; }

            public Point()
            {
                Predicates = new List<Predicate>();
                KeyCaptures = new List<string>();
            }
        }
    }
}
