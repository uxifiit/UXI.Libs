using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Settings
{
    public interface ISettings
    {
        IEnumerable<string> Keys { get; }

        bool TryGetSetting(string key, Type valueType, out object value);
        bool SetSetting(string key, object value);
        void RemoveSetting(string key);

        bool Contains(string key);
        //event ConfigurationSettingChangedEventHandler SettingChanged;

        //event ConfigurationSettingMissingEventHandler SettingMissing;

        //event ConfigurationSettingWriteFailedEventHandler SettingWriteFailed;
    }
}
