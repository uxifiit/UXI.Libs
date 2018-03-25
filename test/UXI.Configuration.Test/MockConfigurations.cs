using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Attributes;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;
using UXI.Configuration.Test.Storages;

namespace UXI.Configuration.Test
{

    [ConfigurationSection(MockStorages.SECTION1_KEY)]
    public class MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1 : ConfigurationBase
    {
        public const string DEFAULT_SECTION = MockStorages.SECTION1_KEY;
        public MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1(IConfigurationSource source) : base(source) { }

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield break; 
            }
        }

        protected override string GetDefaultSectionName()
        {
            return DefaultSectionName = base.GetDefaultSectionName();
        }

        public string DefaultSectionName { get; private set; }
        public ISettings DefaultSectionPublic { get { return DefaultSection; } }

    }

    public class MockConfiguration_Empty_FullTypeNameForDefaultSection : ConfigurationBase
    {
        public static string DEFAULT_SECTION { get { return typeof(MockConfiguration_Empty_FullTypeNameForDefaultSection).FullName; } }
        public const string DEFAULT_SECTION_Explicit = "UXI.Configuration.Test.MockConfiguration_Empty_FullTypeNameForDefaultSection";

        public MockConfiguration_Empty_FullTypeNameForDefaultSection(IConfigurationSource source) : base(source) { }

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield break;
            }
        }

        protected override string GetDefaultSectionName()
        {
            return DefaultSectionName = base.GetDefaultSectionName();
        }

        public string DefaultSectionName { get; private set; }
    }

    [ConfigurationSection(MockStorages.SECTION1_KEY)]
    public class MockConfiguration_Storage1_SectionAttributeForDefaultSection_Section1 : ConfigurationBase
    {
        private readonly IConfigurationSource _source;

        public MockConfiguration_Storage1_SectionAttributeForDefaultSection_Section1(IConfigurationSource source) : base(source) { _source = source; }

        public readonly string Section1 = MockStorages.SECTION1_KEY;

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
            }
        }
        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage1;

        public ConfigurationSettingProperty CreatePropertyPublic(string propertyName, object defaultValue, string sectionName = null)
        {
            return CreateProperty(propertyName, defaultValue, _source, sectionName);                    
        }

        public int IntSetting { get { return IntSettingProperty.Get<int>(); } set { IntSettingProperty.Set(value); } }
        public string StringSetting { get { return StringSettingProperty.Get<string>(); } set { StringSettingProperty.Set(value); } }

        public ConfigurationSettingProperty IntSettingProperty;
        public ConfigurationSettingProperty StringSettingProperty;
    }
}
