using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace ChromeSearch.Shared.Helpers
{
    public static class ResourcesHelper
    {
        public static ResourceLoader ResourceLoader
        {
            get { return new ResourceLoader(); }
        }
    }
}
