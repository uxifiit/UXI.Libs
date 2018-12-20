using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Serialization.Csv.Converters;


namespace UXI.Serialization.Csv
{
    public class CsvSerializationFactory : ISerializationFactory
    {
        //private readonly bool _ignoreDefaultConverters;
        //private readonly Action<Configuration, DataAccess> _configureSerializerCallback;

        public CsvSerializationFactory()
            : this(Enumerable.Empty<ISerializationConfiguration>())
        { }

        //public CsvSerializationFactory(params CsvConverter[] converters)
        //    : this(converters.AsEnumerable())
        //{ }

        //public CsvSerializationFactory(IEnumerable<CsvConverter> converters)
        //    : this(new CsvConvertersConfiguration(converters))
        //{ }

        //public CsvSerializationFactory(IEnumerable<CsvConverter> converters, ISerializationConfiguration configuration)
        //   : this(new CsvConvertersConfiguration(converters), configuration)
        //{ }


        public CsvSerializationFactory(params ISerializationConfiguration[] configurations)
            : this(configurations.AsEnumerable())
        { }


        //public CsvSerializationFactory(IEnumerable<CsvConverter> converters, Func<CsvSerializerContext, DataAccess, object, CsvSerializerContext> configuration)
        //    : this(converters, new RelaySerializationConfiguration<CsvSerializerContext>(configuration))
        //{ }


        public CsvSerializationFactory(IEnumerable<ISerializationConfiguration> configurations)
        {
            Configurations = configurations?.ToList() ?? new List<ISerializationConfiguration>();
        }



        public FileFormat Format => FileFormat.CSV;


        public List<ISerializationConfiguration> Configurations { get; }


        public IDataReader CreateReaderForType(TextReader reader, Type dataType, object settings)
        {
            var serializer = CreateSerializer(DataAccess.Read, settings);

            return new CsvDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, object settings)
        {
            var serializer = CreateSerializer(DataAccess.Write, settings);

            return new CsvDataWriter(writer, dataType, serializer);
        }


        private CsvSerializerContext CreateSerializer(DataAccess access, object settings)
        {
            var serializer = new CsvSerializerContext();

            serializer.Configuration.PrepareHeaderForMatch = header => header.ToLower();
            serializer.Configuration.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            foreach (var configuration in Configurations)
            {
                serializer = (CsvSerializerContext)configuration.Configure(serializer, access, settings)
                           ?? serializer;
            }

            return serializer;
        }


        //private CsvSerializerContext CreateSerializer(DataAccess access, SerializationConfiguration configuration)
        //{
        //    AddConverters(serializer, Converters);

        //    if (_ignoreDefaultConverters == false)
        //    {
        //        SetupDateTimeOffsetSerialization(serializer, configuration.TimestampConverter);
        //        SetupTimestampedDataSerialization(serializer, configuration.TimestampFieldName);

        //        AddConverters(serializer, DefaultConverters);
        //    }
        //}


        //private void SetupDateTimeOffsetSerialization(CsvSerializerContext serializer, ITimestampStringConverter timestampConverter)
        //{
        //    if (timestampConverter != null)
        //    {
        //        serializer.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
        //    }
        //}


        //private void SetupTimestampedDataSerialization(CsvSerializerContext serializer, string timestampFieldName)
        //{
        //    serializer.TimestampFieldName = timestampFieldName;
        //}


        //private static void AddConverters(CsvSerializerContext serializer, IEnumerable<CsvConverter> converters)
        //{
        //    foreach (var converter in converters)
        //    {
        //        serializer.DataConverters.Add(converter);
        //    }
        //}
    }
}