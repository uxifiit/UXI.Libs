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
    class CompositeObjectCsvConverter : CsvConverter<CompositeObject>
    {
        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var single = serializer.Deserialize<SingleIntValue>(reader, naming, nameof(CompositeObject.Single));
            var composite = serializer.Deserialize<MultipleValues>(reader, naming, nameof(CompositeObject.Composite));
            var doubleValue = reader.GetField<double>(naming.Get(nameof(CompositeObject.Double)));

            return new CompositeObject()
            {
                Single = single,
                Composite = composite,
                Double = doubleValue
            };
        }

        protected override void WriteCsv(CompositeObject data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize(writer, data.Single);
            serializer.Serialize(writer, data.Composite);
            writer.WriteField(data.Double);
        }

        protected override void WriteCsvHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<SingleIntValue>(writer, naming, nameof(CompositeObject.Single));
            serializer.WriteHeader<MultipleValues>(writer, naming, nameof(CompositeObject.Composite));
            writer.WriteField(naming.Get(nameof(CompositeObject.Double)));
        }
    }
}
