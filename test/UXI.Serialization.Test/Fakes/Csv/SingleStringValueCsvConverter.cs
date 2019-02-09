﻿using System;
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
        protected override bool TryReadCsv(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref SingleStringValue result)
        {
            string value;

            if (reader.TryGetField<string>(naming.GetDefault("Value"), out value))
            {
                result = new SingleStringValue()
                {
                    Value = value
                };

                return true;
            }

            return false;
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
