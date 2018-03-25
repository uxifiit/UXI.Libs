using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Storages;
using UXI.Configuration.Test.Storages;

namespace UXI.Configuration.Test
{
    public class MockConfigurationInitializer_SingleRuntimeStorage1 : ConfigurationInitializer
    {
        public MockConfigurationInitializer_SingleRuntimeStorage1(IConfigurationSource source) : base(source)
        {
        }

        public readonly string Section1 = MockStorages.SECTION1_KEY;

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
            }
        }
        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage1;
    }

    public class MockConfigurationInitializer_SingleRuntimeStorage1_MOD_A : ConfigurationInitializer
    {
        public MockConfigurationInitializer_SingleRuntimeStorage1_MOD_A(IConfigurationSource source) : base(source)
        {
        }

        public readonly string Section1 = MockStorages.SECTION1_KEY;

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
            }
        }

        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage1_MOD_A;
    }

    public class MockConfigurationInitializer_SingleRuntimeStorage1_MOD_B : ConfigurationInitializer
    {
        public MockConfigurationInitializer_SingleRuntimeStorage1_MOD_B(IConfigurationSource source) : base(source)
        {
        }

        public readonly string Section1 = MockStorages.SECTION1_KEY;

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
            }
        }

        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage1_MOD_B;
    }

    public class MockConfigurationInitializer_SingleRuntimeStorage2 : ConfigurationInitializer
    {
        public MockConfigurationInitializer_SingleRuntimeStorage2(IConfigurationSource source) : base(source)
        {
        }

        public readonly string Section1 = MockStorages.SECTION1_KEY;

        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
            }
        }

        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage2;
    }

    public class MockConfigurationInitializer_MultipleRuntimeStorages : ConfigurationInitializer
    {
        public MockConfigurationInitializer_MultipleRuntimeStorages(IConfigurationSource source) : base(source)
        {
        }

        public readonly string Section1 = MockStorages.SECTION1_KEY;
        public readonly string Section2 = MockStorages.SECTION2_KEY;
        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield return Storage1;
                yield return Storage2;
            }
        }

        public StorageDefinition Storage1 { get; } = MockStorages.SingleSectionStorage1;
        public StorageDefinition Storage2 { get; } = MockStorages.SingleSectionStorage2;
    }

    

    class MockConfigurationInitializer_Empty : ConfigurationInitializer
    {
        protected override IEnumerable<StorageDefinition> Storages
        {
            get
            {
                yield break;
            }
        }

        public MockConfigurationInitializer_Empty(IConfigurationSource source) : base(source) { }
    }
    }
