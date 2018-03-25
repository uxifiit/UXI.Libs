using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXI.Common.Helpers
{
    public static class UriHelper
    {
        public const string RANDOM_PARAMETER_NAME = "random";

        public static Uri AddRandomParameter(Uri uri, string paramName)
        {
            uri.ThrowIfNull(nameof(uri));
            string param = paramName + "=" + DateTime.UtcNow.Ticks.ToString();
            string newUri = uri.AbsoluteUri;

            if ((uri.IsAbsoluteUri && String.IsNullOrWhiteSpace(uri.Query)) || newUri.Contains("?") == false)
            {
                newUri += "?" + param;
            }
            else
            {
                newUri += "&" + param;
            }

            return new Uri(newUri, UriKind.RelativeOrAbsolute);
        }

        public static Uri AddRandomParameter(Uri uri)
        {
            return AddRandomParameter(uri, RANDOM_PARAMETER_NAME);            
        }
    }
}
