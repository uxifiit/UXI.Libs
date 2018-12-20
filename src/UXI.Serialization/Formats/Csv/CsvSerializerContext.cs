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
            return Deserialize(reader, dataType, CsvHeaderNamingContext.Empty);
        }


        public object Deserialize(CsvHelper.CsvReader reader, Type dataType, CsvHeaderNamingContext naming)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanRead);

            //string headerPrefix = prefix;
            //if (String.IsNullOrWhiteSpace(prefix) == false)
            //{
            //    headerPrefix += ReferencePrefix;
            //}

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



        //public T Deserialize<T>(CsvHelper.CsvReader reader, string prefix = null)
        //{
        //    var converter = DataConverters.FirstOrDefault(c => c.CanConvert(typeof(T)) && c.CanRead);

        //    string headerPrefix = prefix;
        //    if (String.IsNullOrWhiteSpace(prefix) == false)
        //    {
        //        headerPrefix += HeaderReferenceDelimiter;
        //    }

        //    return (T)converter.ReadCsv(reader, typeof(T), this, headerPrefix);
        //}


        //public void Serialize(CsvHelper.CsvWriter writer, object data, Type dataType)
        //{
        //    Serialize(writer, data, dataType);
        //}


        public void Serialize(CsvHelper.CsvWriter writer, object data, Type dataType/*, bool completeRecord*/)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanWrite);

            converter.WriteCsv(data, writer, this);

            //if (completeRecord)
            //{
            //    writer.NextRecord();
            //}
        }


        public void Serialize<T>(CsvHelper.CsvWriter writer, T data)
        {
            Serialize(writer, data, typeof(T));
        }


        //public void SerializeReference<T>(CsvHelper.CsvWriter writer, T data)
        //{
        //    Serialize(writer, data, typeof(T));
        //}



        public void WriteHeader(CsvHelper.CsvWriter writer, Type dataType)
        {
            WriteHeader(writer, dataType, CsvHeaderNamingContext.Empty);
        }


        public void WriteHeader(CsvHelper.CsvWriter writer, Type dataType, CsvHeaderNamingContext naming)
        {
            var converter = Converters.FirstOrDefault(c => c.CanConvert(dataType) && c.CanWrite);

            converter.WriteCsvHeader(writer, dataType, this, naming);
            //string headerPrefix = prefix;
            //if (String.IsNullOrWhiteSpace(prefix) == false)
            //{
            //    headerPrefix += ReferencePrefix;
            //}
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

    public class CsvHeaderNamingContext
    {
        public static readonly CsvHeaderNamingContext Empty = new CsvHeaderNamingContext();

        private readonly string _prefix = String.Empty;

        public CsvHeaderNamingContext() { }

        public CsvHeaderNamingContext(string prefix)
        {
            _prefix = prefix;
        }


        public string Get(string name) { return _prefix + name; }


        public CsvHeaderNamingContext GetNextLevel(string name, string delimiter) { return new CsvHeaderNamingContext(_prefix + name + delimiter); }
    }
}
