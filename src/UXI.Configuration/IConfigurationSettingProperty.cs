using System;

namespace UXI.Configuration
{
    public interface IConfigurationSettingProperty : IReadSettingProperty
    {
        string Comment { get; set; }
        string Key { get; }
        Type ValueType { get; }

        void SetValue(object value);
        void Reset();
        bool IsSet { get; }
    }
}
