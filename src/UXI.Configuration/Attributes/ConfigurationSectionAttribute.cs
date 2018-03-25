using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        public string Name { get; }

        public ConfigurationSectionAttribute(string name)
        {
            Name = name;
        }
    }
}
