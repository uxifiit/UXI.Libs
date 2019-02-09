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
    class SingleNullableStructValueCsvConverter : CsvConverter<SingleNullableStructValue>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var value = serializer.Deserialize<CompositeStruct?>(reader, naming, nameof(SingleNullableStructValue.Value));

            return new SingleNullableStructValue()
            {
                Value = value
            };
        }

        protected override void WriteCsv(SingleNullableStructValue data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<CompositeStruct?>(writer, data.Value);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<CompositeStruct?>(writer, naming, nameof(SingleNullableStructValue.Value));
        }
    }
}
