using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXI.Common.Extensions
{
    public static class StreamEx
    {
        public static Task ProgressCopyToAsync(this Stream source, Stream destination, IProgress<int> progress, int bufferSize)
        {
            return ProgressCopyToAsync(source, destination, progress, bufferSize, CancellationToken.None);
        }


        public static Task ProgressCopyToAsync(this Stream source, Stream destination, IProgress<int> progress, int bufferSize, CancellationToken cancellationToken)
        {
            source.ThrowIfNull(nameof(source));

            return ProgressCopyToAsync(source, source.Length, destination, progress, bufferSize, cancellationToken);
        }


        public static async Task ProgressCopyToAsync(this Stream source, long length, Stream destination, IProgress<int> progress, int bufferSize, CancellationToken cancellationToken)
        {
            source.ThrowIfNull(nameof(source));
            destination.ThrowIfNull(nameof(destination));
            bufferSize.ThrowIf(b => b <= 0, () => new ArgumentOutOfRangeException(nameof(bufferSize)));

            var canReportProgress = (length != -1 && progress != null);

            long copied = 0L;
            bool isMoreToCopy = true;
            var buffer = new byte[bufferSize];

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                int readLength = await source.ReadAsync(buffer, 0, buffer.Length);
                if (readLength <= 0)
                {
                    isMoreToCopy = false;
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await destination.WriteAsync(buffer, 0, readLength);

                    copied += readLength;

                    if (canReportProgress)
                    {
                        progress.Report(Convert.ToInt32(((copied * 1d) / length) * 100));
                    }
                }
            } while (isMoreToCopy);
        }
    }
}
