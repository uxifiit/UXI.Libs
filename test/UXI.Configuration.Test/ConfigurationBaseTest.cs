using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UXI.Configuration.Test
{
    [TestClass]
    public class ConfigurationBaseTest
    {
        private IConfigurationSource source;

        [TestInitialize]
        public void Initialize()
        {
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            source = new ConfigurationSource(null);
        }


        [TestMethod]
        public void TestGetDefaultSectionName_Attribute()
        {
            var configuration = new MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1(source);

            Assert.AreEqual(MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1.DEFAULT_SECTION, configuration.DefaultSectionName);
        }


        [TestMethod]
        public void TestGetDefaultSectionName_FullTypeName()
        {
            var configuration = new MockConfiguration_Empty_FullTypeNameForDefaultSection(source);

            Assert.AreEqual(MockConfiguration_Empty_FullTypeNameForDefaultSection.DEFAULT_SECTION, configuration.DefaultSectionName);
            Assert.AreEqual(MockConfiguration_Empty_FullTypeNameForDefaultSection.DEFAULT_SECTION_Explicit, configuration.DefaultSectionName);
        }

        [TestMethod]
        public void TestGetDefaultSection_CreateEmpty()
        {
            var configuration = new MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1(source);

            Assert.IsFalse(configuration.DefaultSectionPublic.Keys.Any());
        }

        [TestMethod]
        public void TestGetDefaultSection_EmptyButExisting()
        {
            var initializer = new MockConfigurationInitializer_SingleRuntimeStorage1(source);
            var configuration = new MockConfiguration_Empty_SectionAttributeForDefaultSection_Section1(source);

            var keys = initializer.Storage1.Storage.GetKeys(configuration.DefaultSectionName).ToList();
            var accessedKeys = configuration.DefaultSectionPublic.Keys.ToList();

            CollectionAssert.IsSubsetOf(accessedKeys, keys);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateProperty_NonExistentProperty()
        {
            var configuration = new MockConfiguration_Storage1_SectionAttributeForDefaultSection_Section1(source);

            var property = configuration.CreatePropertyPublic("missing property name", "default");

            Assert.Fail();
        }

        [TestMethod]
        public void TestCreateProperty_ExistingProperty()
        {
            var configuration = new MockConfiguration_Storage1_SectionAttributeForDefaultSection_Section1(source);

            var property = configuration.CreatePropertyPublic(nameof(configuration.StringSetting), "default");
            configuration.StringSettingProperty = property;

            string actualValue = configuration.StringSetting;
            object expectedValue;
            bool read = configuration.Storage1.Storage.TryRead(configuration.Section1, property.Key, property.ValueType, out expectedValue);

            Assert.IsTrue(read);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
