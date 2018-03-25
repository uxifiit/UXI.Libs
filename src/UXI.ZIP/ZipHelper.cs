using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using UXI.Common.Extensions;

namespace UXI.ZIP
{
    public static class ZipHelper
    {
        public static bool TryPack(string fileName, string folderPath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            try
            {
                return Pack(fileName, folderPath, 0, progress, cancellationToken).Any();
            }
            catch
            {
                return false;
            }
        }


        public static IEnumerable<string> Pack(string fileName, string folderPath, int? segmentSize, IProgress<int> progress, CancellationToken cancellationToken)
        {
            fileName.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(fileName));
            fileName.ThrowIf
            (
                f => Path.GetExtension(fileName).Equals(".exe", StringComparison.InvariantCultureIgnoreCase), 
                nameof(fileName), 
                "Target filename cannot end with the .exe extension"
            ); 

            Action throwExceptionIfOccurred = () => { };

            EventHandler<ZipErrorEventArgs> errorHandler = (_, args) =>
            {
                throwExceptionIfOccurred = () =>
                {
                    throw args.Exception;
                };
            };

            using (ZipFile zip = new ZipFile(fileName, Encoding.UTF8))
            {
                zip.Strategy = Ionic.Zlib.CompressionStrategy.Filtered;
                if (segmentSize.HasValue)
                {
                    zip.MaxOutputSegmentSize = segmentSize.Value;
                }

                zip.ZipErrorAction = ZipErrorAction.InvokeErrorEvent;

                zip.ZipError += errorHandler;

                zip.SaveProgress += (s, p) =>
                {
                    if (p.EntriesTotal > 0)
                    {
                        int portion = Convert.ToInt32((p.EntriesSaved * 1d) / (p.EntriesTotal * 1d) * 100);
                        progress.Report(portion);
                    }
                    p.Cancel = cancellationToken.IsCancellationRequested;
                };

                zip.AddDirectory(folderPath);
                zip.Save();

                zip.ZipError -= errorHandler;

                throwExceptionIfOccurred();

                var outputFiles = Enumerable.Empty<string>();
                string outputFileName = Path.GetFileNameWithoutExtension(fileName);

                if (segmentSize.HasValue && zip.NumberOfSegmentsForMostRecentSave > 0)
                {
                    outputFiles = Enumerable.Range(1, zip.NumberOfSegmentsForMostRecentSave - 1)
                                            .Select(i => $"{outputFileName}.z{i:00}");
                }

                return outputFiles.Append($"{outputFileName}.zip")
                                  .ToList();
            }
        }

        public static bool TryUnpackEntry(string zipPath, string entryName, string targetFolderPath, out string targetPath)
        {
            zipPath.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(zipPath));
            zipPath.ThrowIf(f => File.Exists(f) == false, () => new FileNotFoundException($"File {zipPath} does not exist."));
            zipPath.ThrowIf(f => ZipFile.IsZipFile(f) == false, () => new InvalidDataException($"File {zipPath} is not a zip file."));

            using (ZipFile zip = new ZipFile(zipPath, Encoding.UTF8))
            {
                if (zip.ContainsEntry(entryName))
                {
                    zip.ExtractSelectedEntries($"name = '{entryName}'", null, targetFolderPath, ExtractExistingFileAction.OverwriteSilently);
                    targetPath = Path.Combine(targetFolderPath, entryName);
                    return true;
                }
            }

            targetPath = null;
            return false;
        }


        public static bool Unpack(string zipPath, string targetFolderPath, Encoding encoding, IProgress<int> progress, CancellationToken cancellationToken)
        {
            zipPath.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(zipPath));
            zipPath.ThrowIf(f => File.Exists(f) == false, () => new FileNotFoundException($"File {zipPath} does not exist."));
            zipPath.ThrowIf(f => ZipFile.IsZipFile(f) == false, () => new InvalidDataException($"File {zipPath} is not a zip file."));

            using (ZipFile zip = ZipFile.Read(zipPath, new ReadOptions() { Encoding = Encoding.UTF8 }))
            {
                zip.ExtractProgress += (s, p) =>
                {
                    if (p.EntriesTotal > 0)
                    {
                        int portion = Convert.ToInt32((p.EntriesExtracted * 1d) / (p.EntriesTotal * 1d) * 100);
                        progress.Report(portion);
                    }
                    p.Cancel = cancellationToken.IsCancellationRequested;
                };

                zip.ExtractAll(targetFolderPath, ExtractExistingFileAction.OverwriteSilently);
            }

            return true;
        }
    }
}
