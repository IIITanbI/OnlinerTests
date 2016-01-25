using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace OnlinerTests
{
    public class CatalogPage
    {
        [FindsBy(How = How.XPath, Using = "(//input[@class='schema-filter-control__item schema-filter__number-input schema-filter__number-input_price'])[1]")]
        public IWebElement PriceFromInput { get; set; }

        [FindsBy(How = How.XPath, Using = "(//input[@class='schema-filter-control__item schema-filter__number-input schema-filter__number-input_price'])[2]")]
        public IWebElement PriceToInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".schema-headerasdsad")]
        public IWebElement CatalogHeader { get; set; }

        [FindsBy(How = How.Id, Using = "schema-tags")]
        public IWebElement SearchTag { get; set; }

        [FindsBy(How = How.XPath, Using ="//span[contains(@data-bind, 'minPrice')]")]
        public IList<IWebElement> Prices { get; set; }


        private static CatalogPage instance;

        public CatalogPage(IWebDriver webDriver)
        {
            PageFactory.InitElements(webDriver, this);
        }
        public static CatalogPage Get()
        {
            if (instance == null)
                instance = new CatalogPage(WebDriver.GetDriver());

            return instance;
        }
    }
}
