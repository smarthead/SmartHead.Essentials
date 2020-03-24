using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SmartHead.Essentials.Extensions
{
    public static class StringExtensions
    {
        public static string ToQueryParams(this NameValueCollection nvc)
        {
            var array = nvc
                .AllKeys
                .SelectMany(nvc.GetValues,
                    (key, value) => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}")
                .ToArray();

            return "?" + string.Join("&", array);
        }
    }
}