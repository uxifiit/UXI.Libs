using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace UXI.Common.Web
{
    public class FileActionResult : IHttpActionResult
    {
        public FileActionResult(string path, string filename, string mimeType)
        {
            FilePath = path;
            FileName = filename;
            MimeType = mimeType;
        }

        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string MimeType { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StreamContent(System.IO.File.OpenRead(FilePath));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = FileName
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);

            return Task.FromResult(response);
        }
    }
}
