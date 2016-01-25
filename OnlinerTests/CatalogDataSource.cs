using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinerTests
{
    public static class CatalogDataSource
    {
        public static string BaseUrl = "http://catalog.onliner.by/";
        public static List<string> GetCatalogLinks()
        {
            return new List<string>() { BaseUrl + "dect", BaseUrl+"desktoppc" };

        }
    }
}
