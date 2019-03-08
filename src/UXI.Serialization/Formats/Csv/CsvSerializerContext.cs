using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using UXI.Serialization.Common;
using UXI.Serialization.Csv.Converters;

namespace UXI.Serialization.Csv
{
    public class CsvSerializerContext
    {
        public CsvHelper.Configuration.Configuration Configuration { get; set; } = new CsvHelper.Configuration.Configuration();

        public Collection<CsvConverter> Converters { get; set; } = new Collection<CsvConverter>();

        public string HeaderNestingDelimiter { get; set; } = "";


        public bool TryDeserialize(CsvHelper.CsvReader reader, Type dataType, CsvHeaderNamingContext naming, out object result)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanRead);

            if (converter != null)
            {
                result = converter.ReadCsv(reader, dataType, this, naming);
                return true;
            }

            result = TypeHelper.GetDefault(dataType);
            return false;
        }


        public bool TryDeserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming, out T result)
        {
            object output;

            bool deserialized = TryDeserialize(reader, typeof(T), naming, out output);

            result = (T)output;
            return deserialized;
        }


        public bool TryDeserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming, string referenceName, out T result)
        {
            return TryDeserialize<T>(reader, naming.GetNextLevel(referenceName, HeaderNestingDelimiter), out result);
        }


        public object Deserialize(CsvHelper.CsvReader reader, Type dataType, bool throwIfNoConverterDefined)
        {
            return Deserialize(reader, dataType, new CsvHeaderNamingContext(Configuration.PrepareHeaderForMatch), throwIfNoConverterDefined);
        }


        public object Deserialize(CsvHelper.CsvReader reader, Type dataType, CsvHeaderNamingContext naming, bool throwIfNoConverterDefined)
        {
            object result;
            if (TryDeserialize(reader, dataType, naming, out result))
            {
                return result;
            }
            else if (throwIfNoConverterDefined)
            {
                throw new ArgumentOutOfRangeException(nameof(dataType), $"No converter defined for the requested type '{dataType.FullName}' to deserialize.");
            }
            else
            {
                return TypeHelper.GetDefault(dataType);
            }
        }


        public T Deserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming, bool throwIfNoConverterDefined)
        {
            return (T)Deserialize(reader, typeof(T), naming, throwIfNoConverterDefined);
        }


        public T Deserialize<T>(CsvHelper.CsvReader reader, CsvHeaderNamingContext naming, string referenceName, bool throwIfNoConverterDefined)
        {
            return (T)Deserialize(reader, typeof(T), naming.GetNextLevel(referenceName, HeaderNestingDelimiter), throwIfNoConverterDefined);
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
