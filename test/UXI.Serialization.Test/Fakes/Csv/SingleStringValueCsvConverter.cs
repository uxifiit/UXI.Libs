using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;
using UXI.Serialization.Fakes;

namespace UXI.Serialization.Fakes.Csv.Converters
{
    class SingleStringValueCsvConverter : CsvConverter<SingleStringValue>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var value = reader.GetField<string>(naming.GetDefault("Value"));

            return new SingleStringValue()
            {
                Value = value
            };
        }

        protected override void WriteCsv(SingleStringValue data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Value);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.GetDefault("Value"));
        }
    }
}
