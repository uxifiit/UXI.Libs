using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Settings;
using UXI.Configuration.Test.Storages;

namespace UXI.Configuration.Test
{
    [TestClass]
    public class ConfigurationSourceTest
    {
        private IConfigurationSource source;
        [TestInitialize]
        public void Init()
        {
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            source = new ConfigurationSource(null);
        }

        [TestMethod]
        public void TestHasSection_NoStorage()
        {
            ConfigurationInitializer initializer = new MockConfigurationInitializer_Empty(source);

            Assert.IsFalse(source.Sections.Any());
        }

        [TestMethod]
        public void TestHasSection_SingleRuntimeStorage()
        {
            MockConfigurationInitializer_SingleRuntimeStorage1 initializer = new MockConfigurationInitializer_SingleRuntimeStorage1(source);

            Assert.IsTrue(source.HasSection(initializer.Section1));
        }

        [TestMethod]
        public void TestHasSection_MultipleRuntimeStorages()
        {
            MockConfigurationInitializer_MultipleRuntimeStorages initializer = new MockConfigurationInitializer_MultipleRuntimeStorages(source);

            Assert.IsTrue(source.HasSection(initializer.Section1));
            Assert.IsTrue(source.HasSection(initializer.Section2));
        }


        [TestMethod]
        public void TestHasSection_OverriddenSections()
        {
            var initializer_1 = new MockConfigurationInitializer_SingleRuntimeStorage1_MOD_A(source);

            Assert.IsTrue(source.HasSection(initializer_1.Section1));

            var initializer_2 = new MockConfigurationInitializer_MultipleRuntimeStorages(source);

            Assert.IsTrue(source.HasSection(initializer_1.Section1));

            Assert.IsTrue(source.HasSection(initializer_2.Section1));
            Assert.IsTrue(source.HasSection(initializer_2.Section2));
        }

        [TestMethod]
        public void TestAddStorage_UsesConfigurationSection()
        {
            MockConfigurationInitializer_MultipleRuntimeStorages initializer = new MockConfigurationInitializer_MultipleRuntimeStorages(source);

            var section = source.GetSection(initializer.Section1);

            Assert.IsInstanceOfType(section, typeof(StorageSettings));
        }

        [TestMethod]
        public void TestAddStorage_OverriddenSections()
        {
            MockConfigurationInitializer_SingleRuntimeStorage1_MOD_B initializer_1 = new MockConfigurationInitializer_SingleRuntimeStorage1_MOD_B(source);
            MockConfigurationInitializer_SingleRuntimeStorage1_MOD_A initializer_2 = new MockConfigurationInitializer_SingleRuntimeStorage1_MOD_A(source);
            MockConfigurationInitializer_SingleRuntimeStorage1 initializer_3 = new MockConfigurationInitializer_SingleRuntimeStorage1(source);

            ISettings settings = source.GetSection(initializer_3.Section1);
            StorageSettings section = settings as StorageSettings;

            Assert.IsNotNull(section?.Child?.Child);

            //ConfigurationSection root = section;
            //ConfigurationSection child1 = root.Child;
            //ConfigurationSection child2 = root.Child;

            //var storage = initializer_1.Storage1.Storage;
            //foreach (string sectionName in storage.Sections)
            //{
            //    foreach (string key in storage.GetKeys(sectionName))
            //    {
                    
            //    }
            //}
            //TryRead(initializer_1.Section1

        }
    }
}
