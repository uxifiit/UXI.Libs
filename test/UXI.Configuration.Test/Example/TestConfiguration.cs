using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Settings;

namespace UXI.Configuration.Example
{
    public interface IConfiguration
    {
        IEnumerable<ConfigurationSettingProperty> Properties { get; }
    }

    public interface ITestConfiguration
    {
        int Frequency { get; set; }
    }

    public interface ITestConfigurationProperties
    {
        ConfigurationSettingProperty FrequencyProperty { get; }
    }

    class TestConfiguration : ConfigurationBase, IConfiguration, ITestConfiguration, ITestConfigurationProperties
    {
        public TestConfiguration(IConfigurationSource source) : base(source)
        {
            FrequencyProperty = CreateProperty(nameof(Frequency), 5);



            //FrequencyProperty = new ConfigurationSettingProperty("Frequency", typeof(int), source.GetSection("Test"));   
        }
                   
        public IEnumerable<ConfigurationSettingProperty> Properties
        {
            get
            {
                yield return FrequencyProperty;        
            }
        }

        public int Frequency
        {
            get { return FrequencyProperty.Get<int>(); }
            set { FrequencyProperty.Set(value); }
        }

        public ConfigurationSettingProperty FrequencyProperty { get; } 
    }

    class DependentConfiguration
    {
        public DependentConfiguration(ITestConfigurationProperties properties)
        {
            var storage = new DictionarySettings();
            FrequencyProperty = new ConfigurationSettingProperty(properties.FrequencyProperty, storage);
        }

        private readonly ConfigurationSettingProperty FrequencyProperty;

        public int Frequency
        {
            get { return FrequencyProperty.Get<int>(); }
            set { FrequencyProperty.Set(value); }
        }

        public IEnumerable<ConfigurationSettingProperty> Properties
        {
            get { yield return FrequencyProperty; }
        }
    }
}
