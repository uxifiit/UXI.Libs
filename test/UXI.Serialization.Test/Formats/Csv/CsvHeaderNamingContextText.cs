using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Serialization.Csv
{
    [TestClass]
    public class CsvHeaderNamingContextText
    {

        [TestMethod]
        public void GetNextLevel()
        {
            // TODO Prepare Header Match function is not called on reference name parameter
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Get_NoPrefixNoDelimNoName_ThrowsException()
        {
            var naming = new CsvHeaderNamingContext();

            string name = naming.Get();
        }


        [TestMethod]
        public void Get_NoPrefixNoDelimWithName_ReturnsName()
        {
            var naming = new CsvHeaderNamingContext();

            string name = naming.Get("Column");

            Assert.AreEqual("Column", name);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Get_NoPrefixWithDelimNoName_ThrowsException()
        {
            var naming = new CsvHeaderNamingContext(String.Empty, ".", _ => _);

            string name = naming.Get();
        }


        [TestMethod]
        public void Get_NoPrefixWithDelimWithName_ReturnsName()
        {
            var naming = new CsvHeaderNamingContext(String.Empty, ".", _ => _);

            string name = naming.Get("Column");

            Assert.AreEqual("Column", name);
        }


        [TestMethod]
        public void Get_WithPrefixNoDelimNoName_ReturnsPrefix()
        {
            var naming = new CsvHeaderNamingContext("Object", _ => _);

            string name = naming.Get();

            Assert.AreEqual("Object", name);
        }


        [TestMethod]
        public void Get_WithPrefixNoDelimWithName_ReturnsPrefixName()
        {
            var naming = new CsvHeaderNamingContext("Object", _ => _);

            string name = naming.Get("Column");

            Assert.AreEqual("ObjectColumn", name);
        }


        [TestMethod]
        public void Get_WithPrefixWithDelimNoName_ReturnsPrefix()
        {
            var naming = new CsvHeaderNamingContext("Object", ".", _ => _);

            string name = naming.Get();

            Assert.AreEqual("Object", name);
        }


        [TestMethod]
        public void Get_WithPrefixWithDelimWithName_ReturnsPrefixDelimName()
        {
            var naming = new CsvHeaderNamingContext("Object", ".", _ => _);

            string name = naming.Get("Column");

            Assert.AreEqual("Object.Column", name);
        }


        [TestMethod]
        public void GetNextLevel_WithPrefixWithDelimiterWithName_GetReturnsPrefixDelimName()
        {
            var naming = new CsvHeaderNamingContext();

            var nextLevel = naming.GetNextLevel("Object", ".");

            string name = nextLevel.Get("Column");

            Assert.AreEqual("Object.Column", name);
        }


        [TestMethod]
        public void GetNextLevel_WithPrefixNoDelimiterWithName_GetReturnsPrefixName()
        {
            var naming = new CsvHeaderNamingContext();

            var nextLevel = naming.GetNextLevel("Object");

            string name = nextLevel.Get("Column");

            Assert.AreEqual("ObjectColumn", name);
        }


        [TestMethod]
        public void GetNextLevel_WithPrefixWithDelimiterNoName_GetReturnsPrefix()
        {
            var naming = new CsvHeaderNamingContext();

            var nextLevel = naming.GetNextLevel("Object", ".");

            string name = nextLevel.Get();

            Assert.AreEqual("Object", name);
        }


        [TestMethod]
        public void GetNextLevel_WithPrefixWithDelimiterNoName_Nested_GetReturnsPrefix()
        {
            var naming = new CsvHeaderNamingContext();

            var nextLevel = naming.GetNextLevel("Object", ".");
            var nextNextLevel = nextLevel.GetNextLevel("Nested");

            string name = nextNextLevel.Get();

            Assert.AreEqual("Object.Nested", name);
        }
    }
}
