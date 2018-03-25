using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Configuration.Attributes;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;

namespace UXI.Configuration
{
    public abstract class ConfigurationBase : ConfigurationInitializer//, IConfigurationBase
    {
        //protected Dictionary<string, ISettings> Sections { get; } = new Dictionary<string, ISettings>();

        protected ISettings DefaultSection
        {
            get; set;
        }

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield break;
            }
        }

        //protected ConfigurationBase() : base()
        //{
        //    DefaultSection = new DictionarySettings();
        //}

        //protected ConfigurationBase(bool throwOnMissingKey) : this()
        //{
        //    ThrowOnMissingKey = throwOnMissingKey;
        //}

        protected ConfigurationBase(IConfigurationSource source) : base(source) { InitializeDefaultSection(source); }

        protected ConfigurationBase(IConfigurationSource source, bool throwOnMissingKey) : this(source)
        {
            ThrowOnMissingKey = throwOnMissingKey;
        }

        protected bool ThrowOnMissingKey
        {
            get; set;
        } = false;    // TODO use ThrowOnMissingKey


        protected virtual string GetDefaultSectionName()
        {
            string name;

            if (TryGetSectionFromMemberAttribute(this.GetType(), out name) == false)
            {
                name = this.GetType().FullName;
            }

            return name;
        }

        protected virtual ISettings GetDefaultSection(string name, IConfigurationSource source)
        {
            if (source.HasSection(name) == false)
            {
                source.AddStorage(new RuntimeStorage(name));
            }

            return source.GetSection(name);
        }

        protected virtual string InitializeDefaultSection(IConfigurationSource source)
        {
            string name = GetDefaultSectionName();

            DefaultSection = GetDefaultSection(name, source);

            return name;
        }
        

        //private void RegisterEventHandlersForSection(Settings.ConfigurationSection section)
        //{
        //    section.SettingMissing += OnSettingMissing;
        //    section.SettingChanged += OnSettingChanged;
        //    section.SettingWriteFailed += OnSettingWriteFailed;
        //}

        private static bool TryGetSectionFromMemberAttribute(MemberInfo member, out string section)
        {
            if (Attribute.IsDefined(member, typeof(ConfigurationSectionAttribute), true))
            {
                var typeAttribute = Attribute.GetCustomAttribute(member, typeof(ConfigurationSectionAttribute), true) as ConfigurationSectionAttribute;
                section = typeAttribute.Name;
                return true;
            }
            section = null;
            return false;
        }

        //protected bool TryGetSectionForProperty(string propertyName, out string section)
        //{
        //    return TryGetSectionForMember(this.GetType().GetProperty(propertyName), out section);
        //}

        protected ConfigurationSettingProperty CreateProperty(string propertyName, object defaultValue, IConfigurationSource source, string sectionName = null)
        {
            propertyName.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(propertyName));

            var property = this.GetType()
                .GetProperty(propertyName)
                .ThrowIfNull(() => new ArgumentException($"Property {propertyName} is not defined."));

            ISettings section;
            if ((String.IsNullOrWhiteSpace(sectionName) == false || TryGetSectionFromMemberAttribute(property, out sectionName)) && source.HasSection(sectionName))
            {
                section = source.GetSection(sectionName);
            }
            else
            {
                section = DefaultSection;
            }
                                        
            return new ConfigurationSettingProperty(propertyName, property.PropertyType, defaultValue, section);
        }





        //protected static bool TryGetSectionForProperty(Type type, string propertyName, out string section)
        //{
        //    var property = type.GetProperty(propertyName);
        //    if (property != null && property.IsDefined(typeof(ConfigurationSectionAttribute), true))
        //    {
        //        var propertyAttribute = property.GetCustomAttribute(typeof(ConfigurationSectionAttribute), true) as ConfigurationSectionAttribute;
        //        section = propertyAttribute.Name;
        //        return true;
        //    }

        //    return false;
        //    }

        //private static IEnumerable<string> GetSectionsForProperties(Type type)
        //{
        //    return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //               .Where(p => Attribute.IsDefined(p, typeof(ConfigurationSectionAttribute), true))
        //               .Select(p => Attribute.GetCustomAttribute(p, typeof(ConfigurationSectionAttribute), true))
        //               .Cast<ConfigurationSectionAttribute>()
        //               .Select(a => a.Name)
        //               .Distinct()
        //               .ToArray();
            
        //}


        //protected IConfigurationStorage GetStorageFromAssemblyConfigurationFile(IConfigurationSource source)
        //{
        //    var assembly = Assembly.GetCallingAssembly();
        //    string directory = System.IO.Path.GetDirectoryName(assembly.Location);

        //    string path = System.IO.Path.Combine(directory, assembly.GetName().Name);

        //    AddStorageFromFile(path, source);
        //}

        //protected IConfigurationStorage GetStorageFromFile(string path, IConfigurationSource source)
        //{
        //    IConfigurationStorage storage;
        //    if (ConfigurationStorageHelper.TryFindConfigurationFile(path, out storage))
        //    {
        //        IEnumerable<string> sections = source.AddStorage(storage);

        //        if (sections.Any())
        //        {
        //            TakeSections(source, sections.ToArray());
        //        }
        //    }
        //}

        //protected void GetDefaultSections()
        //{
        //    var properties = this.GetType().GetProperties(BindingFlags.Public);// GetProperty(key);

        //    properties.First().GetCustomAttribute(typeof(ConfigurationSectionAttribute)

        //    if (Attribute.IsDefined(property, typeof(DefaultSettingValueAttribute)))
        //    {
        //        DefaultSettingValueAttribute attribute = (DefaultSettingValueAttribute)Attribute.GetCustomAttribute(property, typeof(DefaultSettingValueAttribute));
        //        value = attribute.Value;
        //        return true;
        //    }
        //    value = null;
        //    return false;

        //}

        //protected void TakeSections(IConfigurationSource source, params string[] names)
        //{
        //    var sections = names.Where(s => source.HasSection(s)).Select(s => source.GetSection(s));
        //    sections.Execute(s => Sections.Add(s.Name, s));

        //    if (DefaultSection == null && sections.Any())
        //    {
        //        DefaultSection = sections.First();
        //    }
        //}


        //protected string Get(string @default, [CallerMemberName] string key = null)
        //{
        //    string value = null; // TODO OnMissingSection
        //    ConfigurationSection section;
        //    string sectionName;

        //    return TryGetSectionForProperty(key, out sectionName) && Sections.TryGetValue(sectionName, out section) && section.TryGetSetting(key, out value) ? value : @default;
        //}

        //protected string Get([CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    ConfigurationSection section;
        //    string sectionName;


        //    if ((TryGetSectionForProperty(key, out sectionName) && Sections.TryGetValue(sectionName, out section) && section.TryGetSetting(key, out value))
        //        || TryGetDefaultSettingValue(key, out value))
        //    {
        //        return value;
        //    }
        //    return null;
        //}

        //protected string GetFromDefaultSection(string @default, [CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    return DefaultSection != null && DefaultSection.TryGetSetting(key, out value) ? value : @default;
        //}

        //protected string GetFromDefaultSection([CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    if ((DefaultSection != null && DefaultSection.TryGetSetting(key, out value)) || TryGetDefaultSettingValue(key, out value))
        //    {
        //        return value;
        //    }
        //    return null;
        //}

        //protected T Get<T>(Func<string, T> convert, [CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    ConfigurationSection section;
        //    string sectionName;

        //    if ((TryGetSectionForProperty(key, out sectionName) && Sections.TryGetValue(sectionName, out section) && section.TryGetSetting(key, out value))
        //        || TryGetDefaultSettingValue(key, out value))
        //    {
        //        return convert.Invoke(value);
        //    }
        //    return default(T);
        //}

        //protected T Get<T>(Func<string, T> convert, T @default, [CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    ConfigurationSection section;
        //    string sectionName;

        //    return TryGetSectionForProperty(key, out sectionName) && Sections.TryGetValue(sectionName, out section) && section.TryGetSetting(key, out value) ? convert.Invoke(value) : @default;
        //}


        //protected T GetFromDefaultSection<T>(Func<string, T> convert, [CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    if ((DefaultSection != null && DefaultSection.TryGetSetting(key, out value))
        //      || TryGetDefaultSettingValue(key, out value))
        //    {
        //        return convert.Invoke(value);
        //    }
        //    return default(T);
        //}


        //protected T GetFromDefaultSection<T>(Func<string, T> convert, T @default, [CallerMemberName] string key = null)
        //{
        //    string value = null;
        //    return DefaultSection != null && DefaultSection.TryGetSetting(key, out value) ? convert.Invoke(value) : @default;
        //}

        //public T Get<T>(Func<string, T> convert, [CallerMemberName] string key = null)
        //{
        //    return convert.Invoke(Get(key));
        //}

        //protected void Set<T>(T value, [CallerMemberName] string key = null)
        //{
        //    Set(Convert.ToString(value), key);
        //}


        //protected void SetToDefault<T>(T value, [CallerMemberName] string key = null)
        //{
        //    SetToDefaultSection(Convert.ToString(value), key);
        //}

        //protected void Set<T>(T value, Func<T, string> convert, [CallerMemberName] string key = null)
        //{
        //    Set(convert.Invoke(value), key);
        //}


        //protected void SetToDefaultSection<T>(T value, Func<T, string> convert, [CallerMemberName] string key = null)
        //{
        //    SetToDefaultSection(convert.Invoke(value), key);
        //}

        //protected void Set(string value, [CallerMemberName] string key = null)
        //{
        //    string sectionName;
        //    ConfigurationSection section;
        //    if (TryGetSectionForProperty(key, out sectionName) && Sections.TryGetValue(sectionName, out section))
        //    {
        //        section.SetSetting(key, value);
        //    }
        //}

        //protected void SetToDefaultSection(string value, [CallerMemberName] string key = null)
        //{
        //    if (DefaultSection != null)
        //    {
        //        DefaultSection.SetSetting(key, value);
        //    }
        //}

        //private bool TryGetDefaultSettingValue(string key, out string value)
        //{
        //    var property = this.GetType().GetProperty(key);
        //    if (Attribute.IsDefined(property, typeof(DefaultSettingValueAttribute)))
        //    {
        //        DefaultSettingValueAttribute attribute = (DefaultSettingValueAttribute)Attribute.GetCustomAttribute(property, typeof(DefaultSettingValueAttribute));
        //        value = attribute.Value;
        //        return true;
        //    }
        //    value = null;
        //    return false;
        //}


        //protected virtual void OnSettingChanged(string sectionName, string settingKey) { }

        //protected virtual void OnSettingMissing(string sectionName, string settingKey)
        //{
        //    if (ThrowOnMissingKey)
        //    {
        //        throw new ConfigurationSettingMissingException(sectionName, settingKey);
        //    }
        //}

        //protected virtual void OnSettingWriteFailed(string sectionName, string settingKey)
        //{
        //    //if (ThrowOnError) // TODO
        //    //{

        //    //}
        //}
    }
}
