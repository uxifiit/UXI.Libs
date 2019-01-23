using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Serialization.Csv
{
    public class CsvHeaderNamingContext
    {
        private readonly Func<string, string> _prepareHeaderName;
        private readonly string _prefix = String.Empty;


        public CsvHeaderNamingContext() { }
        

        public CsvHeaderNamingContext(Func<string, string> prepareHeaderName)
        {
            _prepareHeaderName = prepareHeaderName;
        }


        public CsvHeaderNamingContext(string prefix, Func<string, string> prepareHeaderName)
            : this(prepareHeaderName)
        {
            _prefix = prefix;
        }


        public string Get(string name) { return _prefix + (_prepareHeaderName?.Invoke(name) ?? name); }


        public CsvHeaderNamingContext GetNextLevel(string name, string delimiter) { return new CsvHeaderNamingContext(Get(name) + delimiter, _prepareHeaderName); }
    }
}
