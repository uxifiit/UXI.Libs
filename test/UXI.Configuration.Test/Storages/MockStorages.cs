using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Storages;

namespace UXI.Configuration.Test.Storages
{
    internal static class MockStorages
    {
        public const string SECTION1_KEY = "TestSection";
        public readonly static Dictionary<string, object> SECTION1 = new Dictionary<string, object>()
        {
            { "string property", "a" },
            { "string property 2", "asdasdad asd asf asf asf asf asfasf" },
            { "int property", "235345" },
            { "bool property", "true" },
            { "IntSetting", "455" },
            { "StringSetting", "asfa 2 aw 334a4" }
        };

        public readonly static Dictionary<string, object> SECTION1_MOD_A = new Dictionary<string, object>()
        {
            { "string property", "abfdgfdg" },
            { "string property 2", "asdasdadsdfsf asd asf asf asf asf asfasf" },
            { "int property", "2045" },
            { "bool property", "false" },
            { "IntSetting", "-4547445" },
            { "StringSetting", "asfa 2 aw  sad asf awrr 334a4" }
        };

        public readonly static Dictionary<string, object> SECTION1_MOD_B = new Dictionary<string, object>()
        {
            { "string property", "abfdgfdg" },
        };

        public readonly static StorageDefinition SingleSectionStorage1 = StorageDefinition.Create(new RuntimeStorage(SECTION1_KEY, SECTION1));

        public readonly static StorageDefinition SingleSectionStorage1_MOD_A = StorageDefinition.Create(new RuntimeStorage(SECTION1_KEY, SECTION1_MOD_A));
        public readonly static StorageDefinition SingleSectionStorage1_MOD_B = StorageDefinition.Create(new RuntimeStorage(SECTION1_KEY, SECTION1_MOD_B));

        public const string SECTION2_KEY = "TestSection2";
        public readonly static Dictionary<string, object> SECTION2 = new Dictionary<string, object>() {
            { "string property 2", "asdasdad asd asf asf asf asf asfasf" },
            { "int property", "235345" },
            { "IntSetting", "455" },
            { "StringSetting", "asfa 2 aw 334a4" }
        };



        public readonly static StorageDefinition SingleSectionStorage2 = StorageDefinition.Create(new RuntimeStorage(SECTION2_KEY, SECTION2));

        public readonly static StorageDefinition MultipleSectionStorage1_2 = StorageDefinition.Create(new RuntimeStorage(
            new Dictionary<string, IEnumerable<KeyValuePair<string, object>>>()
            {
                { SECTION1_KEY, SECTION1 },
                { SECTION2_KEY, SECTION2 }
            }));

    }
}
