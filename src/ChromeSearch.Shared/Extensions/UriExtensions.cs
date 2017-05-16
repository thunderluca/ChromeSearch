using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class UriExtensions
    {
        public static IDictionary<string, string> GetQueryParameters(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (string.IsNullOrWhiteSpace(uri.Query))
                return new Dictionary<string, string>();

            var query = uri.Query;
            if (query.StartsWith("?"))
                query = query.Substring(1);

            if (!query.Contains("&"))
                return new[] { GetParamKeyValuePair(query) }.ToDictionary();

            return query.Split('&').Select(GetParamKeyValuePair).ToDictionary();
        }

        private static KeyValuePair<string, string> GetParamKeyValuePair(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new KeyValuePair<string, string>(string.Empty, string.Empty);

            var splittedString = s.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (splittedString.Where(str => !string.IsNullOrWhiteSpace(str)).Count() != 2)
                return new KeyValuePair<string, string>(string.Empty, string.Empty);

            return new KeyValuePair<string, string>(splittedString[0], Net.WebUtility.UrlDecode(splittedString[1]));
        }
    }
}
