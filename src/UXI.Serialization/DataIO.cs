using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Serialization
{
    public class DataIO
    {
        private readonly Dictionary<FileFormat, ISerializationFactory> _formats;


        public DataIO(params ISerializationFactory[] factories)
            : this(factories?.AsEnumerable())
        {
        }


        public DataIO(IEnumerable<ISerializationFactory> factories)
        {
            _formats = factories?.ToDictionary(f => f.Format) ?? new Dictionary<FileFormat, ISerializationFactory>();
        }


        public IEnumerable<object> ReadInput(string filePath, FileFormat fileFormat, Type dataType, object settings)
        {
            FileFormat format = EnsureCorrectFileFormat(filePath, fileFormat);

            using (var reader = FileHelper.CreateInputReader(filePath))
            using (var dataReader = GetInputDataReader(reader, format, dataType, settings))
            {
                object data;
                while (dataReader.TryRead(out data))
                {
                    yield return data;
                }

                yield break;
            }
        }


        public IEnumerable<T> ReadInput<T>(string filePath, FileFormat fileFormat, object settings)
        {
            return ReadInput(filePath, fileFormat, typeof(T), settings).OfType<T>();
        }


        public IDataReader GetInputDataReader(TextReader reader, FileFormat fileType, Type dataType, object settings)
        {
            ISerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateReaderForType(reader, dataType, settings);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


     
        public void WriteOutput(IEnumerable<object> data, string filePath, FileFormat fileFormat, Type dataType, object settings)
        {
            FileFormat format = EnsureCorrectFileFormat(filePath, fileFormat);

            using (var writer = FileHelper.CreateOutputWriter(filePath))
            using (var dataWriter = GetOutputDataWriter(writer, format, dataType, settings))
            {
                foreach (var item in data)
                {
                    dataWriter.Write(item);
                }

                dataWriter.Close();
            }
        }


        public IDataWriter GetOutputDataWriter(TextWriter writer, FileFormat fileType, Type dataType, object settings)
        {
            ISerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateWriterForType(writer, dataType, settings);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


        public FileFormat EnsureCorrectFileFormat(string filename, FileFormat requestedFormat)
        {
            string extension = Path.GetExtension(filename)?.ToLower();

            if (String.IsNullOrWhiteSpace(extension) == false)
            {
                var matchingFormat = _formats.Where(f => f.Key.ToString().ToLower() == extension)
                                             .Select(f => f.Value)
                                             .FirstOrDefault();

                return matchingFormat != null && matchingFormat.Format != requestedFormat
                     ? matchingFormat.Format
                     : requestedFormat;
            }

            return requestedFormat;
        }
    }
}
