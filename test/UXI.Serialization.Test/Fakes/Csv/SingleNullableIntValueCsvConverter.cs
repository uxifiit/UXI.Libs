using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.Serialization.Fakes.Csv.Converters
{
    class SingleNullableIntValueCsvConverter : CsvConverter<SingleNullableIntValue>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            int? value = reader.GetField<int?>(naming.GetDefault(nameof(SingleNullableIntValue.Value)));

            return new SingleNullableIntValue()
            {
                Value = value
            };
        }

        protected override void WriteCsv(SingleNullableIntValue data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Value);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.GetDefault(nameof(SingleNullableIntValue.Value)));
        }
    }
}
