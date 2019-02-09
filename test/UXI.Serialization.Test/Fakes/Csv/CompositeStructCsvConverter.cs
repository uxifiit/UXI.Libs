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
    class CompositeStructCsvConverter : CsvConverter<CompositeStruct>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var id = reader.GetField<int>(naming.Get(nameof(CompositeStruct.Id)));
            var duration = reader.GetField<double>(naming.Get(nameof(CompositeStruct.Duration)));

            return new CompositeStruct()
            {
                Id = id,
                Duration = duration
            };
        }

        protected override void WriteCsv(CompositeStruct data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Id);
            writer.WriteField(data.Duration);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(CompositeStruct.Id)));
            writer.WriteField(naming.Get(nameof(CompositeStruct.Duration)));
        }
    }
}
