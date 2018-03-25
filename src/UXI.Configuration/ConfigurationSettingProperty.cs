using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Configuration.Settings;

namespace UXI.Configuration
{


    public class ConfigurationSettingProperty : IConfigurationSettingProperty, IReadSettingProperty
    {
        private readonly IReadSettingProperty _default;
        private readonly ISettings _storage;

        public ConfigurationSettingProperty
            (
            string key,
            object value
            )
            : this(key, value.ThrowIfNull(nameof(value)).GetType(), value.GetType().GetDefault(), new SingleSetting(key))
        {
            SetValue(value);
        }

        public ConfigurationSettingProperty
            (
            string key,
            object value,
            ISettings storage
            )
            : this(key, value.ThrowIfNull(nameof(value)).GetType(), value.GetType().GetDefault(), storage)
        {
            SetValue(value);
        }

        public ConfigurationSettingProperty
            (
            string key,
            Type type,
            object defaultValue
            )
            : this(key, type, defaultValue, new SingleSetting(key))
        {
           
        }

        public ConfigurationSettingProperty
            (
            string key,
            Type type,
            object defaultValue,
            ISettings storage
            )
        {
            Key = key;
            ValueType = type.ThrowIfNull(nameof(type));

            defaultValue.ThrowIf(
               v => IsValueOfType(v, type) == false && CanCastValueToType(v, type) == false,
               nameof(defaultValue),
               "Type of the default value must be the same as the type of this property.");

            _default = new ConstantProperty(defaultValue);

            _storage = storage.ThrowIfNull(nameof(storage));
        }

        public ConfigurationSettingProperty
            (
            string key,
            IConfigurationSettingProperty source
            )
            : this(key, source, new SingleSetting(key))
        {

        }

        public ConfigurationSettingProperty
           (
           string key,
           IConfigurationSettingProperty source,
           ISettings storage
           )
        {
            Key = key;
            ValueType = source.ValueType;

            _default = source;

            _storage = storage.ThrowIfNull(nameof(storage));
        }

        public ConfigurationSettingProperty
           (
           IConfigurationSettingProperty source
           )
           : this(source.Key, source)
        {
        }

        public ConfigurationSettingProperty
           (
           IConfigurationSettingProperty source,
           ISettings storage
           )
           : this(source.Key, source, storage)
        {
        }

        public string Key { get; private set; }

        public Type ValueType { get; }

        public object GetValue()
        {
            object value;
            if (_storage.TryGetSetting(Key, ValueType, out value))
            {
                return Convert.ChangeType(value, ValueType, CultureInfo.InvariantCulture);
            }
            else
            {
                return Convert.ChangeType(_default.GetValue(), ValueType, CultureInfo.InvariantCulture);
            }
        }

        public void SetValue(object value)
        {
            value.ThrowIf(v => IsValueOfType(v, ValueType) == false, () => new InvalidCastException());

            _storage.SetSetting(Key, value);
        }

        public bool IsSet => _storage.Contains(Key);

        public T Get<T>()
        {
            var value = this.GetValue();

            return (T)value;
        }

        public void Set<T>(T value)
        {
            this.SetValue(value);
        }

        public void Reset()
        {
            _storage.RemoveSetting(Key);
        }

        public string Comment { get; set; }

        private static bool IsValueOfType(object value, Type type)                                             // BUG: does not check nullable types correctly typeof(DateTime?).IsValueType === true
        {
            return (type.IsValueType == false && value == null) || type.IsInstanceOfType(value);
        }

        private static bool CanCastValueToType(object value, Type type)
        {
            try
            {
                Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                return true;
            }
            catch { }

            return false;
        }

        private class SingleSetting : ISettings
        {
            private readonly string _key;

            private bool _set = false;
            private object _value = null;

            internal SingleSetting(string key)
            {
                _key = key;
            }

            public bool TryGetSetting(string key, Type valueType, out object value)
            {
                value = _value;
                return _set;
            }

            public bool SetSetting(string key, object value)
            {
                _value = value;
                _set = true;
                return true;
            }

            public void RemoveSetting(string key)
            {
                _set = false;
                _value = null;
            }

            public bool Contains(string key)
            {
                return _set;
            }

            public IEnumerable<string> Keys { get { yield return _key; } }
        }

        private class ConstantProperty : IReadSettingProperty
        {
            private readonly object _value;
            internal ConstantProperty(object value)
            {
                _value = value;
            }

            public object GetValue()
            {
                return _value;
            }
        }
    }
}
