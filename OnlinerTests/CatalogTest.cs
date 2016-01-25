using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NLog;
using System.Threading;

namespace OnlinerTests
{
    [TestFixture]
    public class CatalogTest
    {
        public int MinPrice { get; set; } = 100000;
        public int MaxPrice { get; set; } = 20000000;

        public Logger Log = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void Init()
        {
            WebDriver.GetDriver().Navigate().GoToUrl("http://www.onliner.by/");
            Log.Info("Go to page http://www.onliner.by/");
        }

        [Test]
        [TestCaseSource(typeof(CatalogDataSource), "GetCatalogLinks")]
        public void CheckPriceRange(string catalogLink)
        {
            WebDriver.GetDriver().Navigate().GoToUrl(catalogLink);
            Log.Info("Go to catalog " + catalogLink);

            Log.Info("Wait untill catalog header is visible");
            WebDriver.WaitUntilElementIsVisible(CatalogPage.Get().CatalogHeader);
            Log.Info("catalog header is visible");

            Log.Info("Set min price");
            CatalogPage.Get().PriceFromInput.SendKeys(MinPrice.ToString());

            Log.Info("Set max price");
            CatalogPage.Get().PriceToInput.SendKeys(MaxPrice.ToString());

            Thread.Sleep(2000);

            Log.Info("Wait untill search tag is visible");
            WebDriver.WaitUntilElementIsVisible(CatalogPage.Get().SearchTag);
            Log.Info("search tag is visible");

            Log.Info("enumerate prices");
            foreach (var value in CatalogPage.Get().Prices)
            {
                string text = value.Text;
                int pos = text.IndexOf("руб");
                text = text.Substring(0, pos).Replace(" ", "");
                
                int price = int.Parse(text);

                Assert.That(price >= MinPrice && price <= MaxPrice, $"price {price} out of range {MinPrice} - {MaxPrice}");
            }

        }
    }
}
