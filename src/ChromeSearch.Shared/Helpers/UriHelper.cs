using System;
using System.Collections.Generic;
using System.Text;

namespace ChromeSearch.Shared.Helpers
{
    public static class UriHelper
    {
        public static void ClearCookiesFor(Uri uri)
        {
            var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieManager = httpFilter.CookieManager;
            var myCookieJar = cookieManager.GetCookies(uri);
            foreach (var cookie in myCookieJar)
                cookieManager.DeleteCookie(cookie);
        }
    }
}
