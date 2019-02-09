using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXI.Serialization.Fakes;
using UXI.Serialization.Fakes.Csv.Converters;

namespace UXI.Serialization.Csv.Converters
{
    [TestClass]
    public class CsvConverterTest
    {
        [TestMethod]
        public void WriteHeader_SingleIntValue_ColumnsIs1()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleIntValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleIntValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            Assert.AreEqual(1, converter.Columns);
        }


        [TestMethod]
        public void WriteHeader_CompositeValue_ColumnsIs2()
        {
            var serializer = new CsvSerializerContext();
            var converter = new CompositeValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(MultipleValues), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            Assert.AreEqual(2, converter.Columns);
        }


        [TestMethod]
        public void WriteHeader_CompositeObject_ColumnsIs4()
        {
            var serializer = new CsvSerializerContext();
            var converter = new CompositeObjectCsvConverter();
            serializer.Converters.Add(converter);
            serializer.Converters.Add(new SingleIntValueCsvConverter());
            serializer.Converters.Add(new CompositeValueCsvConverter());

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(CompositeObject), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            Assert.AreEqual(4, converter.Columns);
        }


        [TestMethod]
        public void WriteCsv_SingleIntValue_NullRecordOnly_EmptyRecord()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleIntValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleIntValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(null, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Value",    // header
                "",         // empty record
                ""          // empty line after the last record
            };


            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }



        [TestMethod]
        public void WriteCsv_SingleIntValue_ValidRecords()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleIntValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleIntValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 3 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 4 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 5 }, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Value",    // header
                "3",        // first record
                "4",        // second record
                "5",        // third record
                ""          // empty line after the last record
            };


            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }



        [TestMethod]
        public void WriteCsv_SingleIntValue_ValidRecordsWithNullRecord()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleIntValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleIntValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 3 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(null, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 4 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleIntValue() { Value = 5 }, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Value",    // header
                "3",        // first record
                "",         // second empty record
                "4",        // third record
                "5",        // fourth record
                ""          // empty line after the last record
            };


            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }


        [TestMethod]
        public void WriteCsv_CompositeObject_ValidRecords()
        {
            var serializer = new CsvSerializerContext();
            var converter = new CompositeObjectCsvConverter();
            serializer.Converters.Add(converter);
            serializer.Converters.Add(new SingleIntValueCsvConverter());
            serializer.Converters.Add(new CompositeValueCsvConverter());

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(CompositeObject), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = new MultipleValues() { Id = 1, Name = "Apple" }, Single = new SingleIntValue() { Value = 15 }, Double = 52.1 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = new MultipleValues() { Id = 2, Name = "Strawberry" }, Single = new SingleIntValue() { Value = 158792 }, Double = -15.2 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = new MultipleValues() { Id = 3, Name = "Orange" }, Single = new SingleIntValue() { Value = 0 }, Double = 0 }, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Single,CompositeId,CompositeName,Double",    // header
                "15,1,Apple,52.1",                             // first record
                "158792,2,Strawberry,-15.2",                   // second record
                "0,3,Orange,0",                                // third record
                ""                                             // empty line after the last record
            };


            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }


        [TestMethod]
        public void WriteCsv_CompositeObject_RecordsWithNullValues()
        {
            var serializer = new CsvSerializerContext();
            var converter = new CompositeObjectCsvConverter();
            serializer.Converters.Add(converter);
            serializer.Converters.Add(new SingleIntValueCsvConverter());
            serializer.Converters.Add(new CompositeValueCsvConverter());

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(CompositeObject), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = new MultipleValues() { Id = 1, Name = null }, Single = new SingleIntValue() { Value = 15 }, Double = 52.1 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = new MultipleValues() { Id = 2, Name = "Strawberry" }, Single = null, Double = -15.2 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new CompositeObject() { Composite = null, Single = new SingleIntValue() { Value = 0 }, Double = 0 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(null, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Single,CompositeId,CompositeName,Double",    // header
                "15,1,,52.1",                                  // first record
                ",2,Strawberry,-15.2",                         // second record
                "0,,,0",                                       // third record
                ",,,",                                         // fourth null record
                ""                                             // empty line after the last record
            };


            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }

        
        [TestMethod]
        public void WriteCsv_ClassWithNullablePrimitive_RecordsWithNullValues()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleNullableIntValueCsvConverter();
            serializer.Converters.Add(converter);

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleNullableIntValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableIntValue() { Value = 1 }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableIntValue() { Value = null }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableIntValue() { Value = 2 }, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "Value",        // header
                "1",            // first record
                "",             // second empty record
                "2",            // third record
                ""              // empty line after the last record
            };

            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }


        [TestMethod]
        public void WriteCsv_ClassWithNullableCompositeStruct_RecordsWithNullValues()
        {
            var serializer = new CsvSerializerContext();
            var converter = new SingleNullableStructValueCsvConverter();
            serializer.Converters.Add(converter);
            serializer.Converters.Add(new CompositeStructCsvConverter());

            var target = new StringWriter();
            var writer = new CsvWriter(target, serializer.Configuration);

            converter.WriteCsvHeader(writer, typeof(SingleNullableStructValue), serializer, new CsvHeaderNamingContext());
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableStructValue() { Value = new CompositeStruct() { Id = 1, Duration = 15.24 } }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableStructValue() { Value = null }, writer, serializer);
            writer.NextRecord();

            converter.WriteCsv(new SingleNullableStructValue() { Value = new CompositeStruct() { Id = 2, Duration = -99.24 } }, writer, serializer);
            writer.NextRecord();

            writer.Dispose();
            target.Close();

            string[] expected = new string[] {
                "ValueId,ValueDuration",        // header
                "1,15.24",                      // first record
                ",",                            // second empty record
                "2,-99.24",                     // third record
                ""                              // empty line after the last record
            };

            Assert.AreEqual(String.Join(Environment.NewLine, expected), target.ToString());
        }
    }
}
