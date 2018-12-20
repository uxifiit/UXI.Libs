using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace UXI.Serialization.Csv.Converters
{
    public abstract class CsvConverter
    {
        public virtual bool CanRead => true;

        public virtual bool CanWrite => true;

        public abstract bool CanConvert(Type objectType);

        public virtual void ReadCsvHeader(CsvReader reader, CsvSerializerContext serializer)
        {
            reader.ReadHeader();
        }

        public abstract void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming);

        public abstract void WriteCsv(object data, CsvWriter writer, CsvSerializerContext serializer);

        public abstract object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming);
    }



    public abstract class CsvConverter<T> : CsvConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }


        protected abstract void WriteCsv(T data, CsvWriter writer, CsvSerializerContext serializer);


        public sealed override void WriteCsv(object data, CsvWriter writer, CsvSerializerContext serializer)
        {
            if (data is T)
            {
                try
                {
                    WriteCsv((T)data, writer, serializer);
                }
                catch (Exception exception)
                {
                    throw new SerializationException($"Failed to write or serialize next data to the CSV file. See inner exception for more details.", exception);
                }
            }
            else
            {
                throw new ArgumentException($"Type of the passed object [{data?.GetType().FullName}] does not match the type supported by this converter [{typeof(T).FullName}].");
            }
        }       
    }
}
