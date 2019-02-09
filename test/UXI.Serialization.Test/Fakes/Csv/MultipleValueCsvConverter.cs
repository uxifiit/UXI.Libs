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
    class CompositeValueCsvConverter : CsvConverter<MultipleValues>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var id = reader.GetField<int>(naming.Get(nameof(MultipleValues.Id)));
            var name = reader.GetField<string>(naming.Get(nameof(MultipleValues.Name)));

            return new MultipleValues()
            {
                Id = id,
                Name = name
            };
        }

        protected override void WriteCsv(MultipleValues data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Id);
            writer.WriteField(data.Name);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(MultipleValues.Id)));
            writer.WriteField(naming.Get(nameof(MultipleValues.Name)));
        }
    }
}
