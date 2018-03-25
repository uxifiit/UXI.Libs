using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Web.Extensions
{
    public static class HttpClientEx
    {
        public static void DisableCaching(this HttpClient client)
        {
            client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
        }
    }
}
