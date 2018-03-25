using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UXI.Common.Web.Extensions
{
    public static class HttpRequestBaseEx
    {

        public static void ThrowIfDifferentReferrer(this HttpRequestBase request)
        {
            Uri referrer = request.UrlReferrer;
            if (referrer == null || request.Url.Host.Equals(referrer.Host, StringComparison.InvariantCultureIgnoreCase) == false)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
