using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common
{
    public class ArgumentValue
    {
        public string Name { get; }
        public object Value { get; }
        public ArgumentValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name + " = " + Value.ToString();
        }
    }
}
