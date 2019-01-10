using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using UXI.Serialization.Csv.Converters;

namespace UXI.Serialization.Csv
{
    public class CsvSerializerContext
    {
        public Configuration Configuration { get; set; } = new CsvHelper.Configuration.Configuration();

        public Collection<CsvConverter> Converters { get; set; } = new Collection<CsvConverter>();

        public string HeaderNestingDelimiter { get; set; } = "";


        public object Deserialize(CsvHelper.CsvReader reader, Type dataType)
        {
            return Deserialize(reader, dataType, new CsvHeaderNamingContext(Configuration.PrepareHeaderForMatch));
        }


        public object Deserialize(CsvHelper.CsvReader reader, Type dataType, CsvHeaderNamingContext naming)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanRead);

            if (converter == null)
            {
                throw new ArgumentOutOfRangeException(nameof(dataType), $"No converter defined for the requested type '{dataType.FullName}' to deserialize.");
            }

            return converter.ReadCsv(reader, dataType, this, naming);
        }


        public T Deserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming)
        {
            return (T)Deserialize(reader, typeof(T), naming);
        }


        public T Deserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming, string referenceName)
        {
            return (T)Deserialize(reader, typeof(T), naming.GetNextLevel(referenceName, HeaderNestingDelimiter));
        }


        public void Serialize(CsvHelper.CsvWriter writer, object data, Type dataType)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanWrite);

            converter.WriteCsv(data, writer, this);
        }


        public void Serialize<T>(CsvHelper.CsvWriter writer, T data)
        {
            Serialize(writer, data, typeof(T));
        }


        public void WriteHeader(CsvHelper.CsvWriter writer, Type dataType)
        {
            WriteHeader(writer, dataType, new CsvHeaderNamingContext(Configuration.PrepareHeaderForMatch));
        }


        public void WriteHeader(CsvHelper.CsvWriter writer, Type dataType, CsvHeaderNamingContext naming)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanWrite);

            converter.WriteCsvHeader(writer, dataType, this, naming);
        }


        public void WriteHeader<T>(CsvHelper.CsvWriter writer, CsvHeaderNamingContext naming)
        {
            WriteHeader(writer, typeof(T), naming);
        }


        public void WriteHeader<T>(CsvHelper.CsvWriter writer, CsvHeaderNamingContext naming, string referenceName)
        {
            WriteHeader<T>(writer, naming.GetNextLevel(referenceName, HeaderNestingDelimiter));
        }


        public void ReadHeader(CsvHelper.CsvReader reader, Type dataType)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanRead);

            converter.ReadCsvHeader(reader, this);
        }
    }
}
