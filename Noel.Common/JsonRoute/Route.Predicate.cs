using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Common.JsonRoute
{
    public partial class JsonRoute
    {
        public abstract class Predicate
        {
            public static Predicate Parse(string l, string op, string r)
            {
                switch (op)
                {
                    case "=": return new Eq { Key = l, Condition = r };
                    case "!=": return new Neq { Key = l, Condition = r };
                }

                throw new Exception($"Invalid predicate '{l}{op}{r}'");
            }

            protected Predicate() { }

            private class Eq : Predicate
            {
                public override bool IsMatch(string value) => value == Condition;
            }

            private class Neq : Predicate
            {
                public override bool IsMatch(string value) => value != Condition;
            }

            public abstract bool IsMatch(string value);

            public string Key { get; private set; }
            private string Condition { get; set; }
        }
    }
}
