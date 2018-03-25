using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXI.Common.Web.Extensions
{
    public static class HttpResponseMessageEx
    {
        public static async Task ProgressReadToStreamAsync(this HttpResponseMessage response, Stream targetStream, IProgress<int> progress, CancellationToken cancellationToken, int bufferSize = 4096)
        {
            response.EnsureSuccessStatusCode();

            long totalLength = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                await stream.ProgressCopyToAsync(totalLength, targetStream, progress, bufferSize, cancellationToken);
            }
        }
    }
}
