using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace OnlinerTests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IWebDriver Driver { get; private set; } = new FirefoxDriver();
        protected Logger Log { get; private set; } = LogManager.GetCurrentClassLogger();
        
        [TestFixtureSetUp]
        public abstract void Init();
        [TestFixtureTearDown]
        public abstract void Finish();
    }

    [TestFixture]
    public class OnlinerTest : TestBase
    {
        public const string BaseUrl = "http://www.onliner.by";
        [Test]
        public void Start()
        {
            Catalog();
            AllCatalog();
        }

        public override void Init()
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Driver.Navigate().GoToUrl(BaseUrl);
        }

        public void Catalog()
        {
            Driver.FindElement(By.XPath(".//*[@id='container']/div/div[2]/header/nav/ul[2]/li[1]/a")).Click();
        }

        public void AllCatalog()
        {
            int min = 30000000;
            int max = 300000000;

            var categories = Driver.FindElements(By.ClassName("catalog-navigation-classifier__item-title-wrapper"));
            var count = categories.Count;

            string tt = "(//li[contains(@class, 'catalog-navigation-classifier__item')])";
            string sub_tt = "(//span[contains(@class, 'catalog-navigation-list__link-inner')])";

            int j = 1;
            for (int i = 1; i <= count; i++)
            {
                var elem = Driver.FindElement(By.XPath(tt + "[" + i + "]"));
                elem.Click();
                Thread.Sleep(2000);
                CheckCategory(i);
                //var sub_categories = Driver.FindElements(By.XPath("(//span[contains(@class, 'catalog-navigation-list__link-inner')])"));
                //var sub_count = sub_categories.Count;
                //Log.Info("sub_count -= " + sub_count);

                //if (i == 1)
                //    j = 85;
                //for (; j <= sub_count; j++)
                //{

                //    var sub_elem = Driver.FindElement(By.XPath(sub_tt + "[" + j + "]"));
                //    Log.Info(sub_elem.Displayed);
                //    if (!sub_elem.Displayed) break;

                //    sub_elem.Click();
                //    SetMinPrice(min);
                //    SetMaxPrice(max);
                //    Check(min, max);

                //    elem = Driver.FindElement(By.XPath(tt + "[" + i + "]"));
                //    elem.Click();
                //    Thread.Sleep(500);
                //}
                //Log.Info("END");

            }
        } 
        public void CheckCategory(int i)
        {
            int min = 30000000;
            int max = 300000000;

            var categories = Driver.FindElements(By.ClassName("catalog-navigation-classifier__item-title-wrapper"));
            var count = categories.Count;

            string tt = "(//li[contains(@class, 'catalog-navigation-classifier__item')])";
            string sub_tt = "(//span[contains(@class, 'catalog-navigation-list__link-inner')])";



            var sub_categories = Driver.FindElements(By.XPath("(//span[contains(@class, 'catalog-navigation-list__link-inner')])"));
            var sub_count = sub_categories.Count;
            Log.Info("sub_count -= " + sub_count);

            for (int j = 1; j <= sub_count; j++)
            {

                var sub_elem = Driver.FindElement(By.XPath(sub_tt + "[" + j + "]"));
                Log.Info(sub_elem.Displayed);
                if (!sub_elem.Displayed) break;

                sub_elem.Click();
                SetMinPrice(min);
                SetMaxPrice(max);
                Check(min, max);

                var elem = Driver.FindElement(By.XPath(tt + "[" + i + "]"));
                elem.Click();
                Thread.Sleep(500);
            }
            Log.Info("END");


        }
        public void ComputersAndNetworks()
        {
            //Driver.FindElement(By.LinkText("Компьютеры и сети")).Click();

            Driver.FindElement(By.XPath("//li[@class='catalog-navigation-classifier__item' and @data-id='2']")).Click();
        }
        public void Computers()
        {
            Driver.FindElement(By.XPath(".//*[@id='container']/div/div[2]/div/div/div[1]/div[2]/div/div[2]/div[2]/ul/li[1]/span/a")).Click();
        }

        public void SetMinPrice(int price)
        {
            //Driver.FindElement(By.XPath(".//*[@id='schema-filter']/div[1]/div[2]/div[2]/div/div[1]/input")).SendKeys(price.ToString());
            Driver.FindElement(By.XPath("(//input[@class='schema-filter-control__item schema-filter__number-input schema-filter__number-input_price'])[1]")).SendKeys(price.ToString());
        }
        public void SetMaxPrice(int price)
        {
            //Driver.FindElement(By.XPath(".//*[@id='schema-filter']/div[1]/div[2]/div[2]/div/div[2]/input")).SendKeys(price.ToString());
            Driver.FindElement(By.XPath("(//input[@class='schema-filter-control__item schema-filter__number-input schema-filter__number-input_price'])[2]")).SendKeys(price.ToString());
        }


        public void Check(int min, int max)
        {

            //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            //var elements = wait.Until(driver => driver.FindElements(By.ClassName("schema-product__price")));

            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            Thread.Sleep(3000);

            while(true)
            {
                var elements = Driver.FindElements(By.ClassName("schema-product__price"));
                Log.Info(elements.Count);
                foreach (var element in elements)
                {
                    var tt = element.FindElement(By.TagName("span"));
                    string text = tt.Text;
                    int pos = text.IndexOf("руб");
                    text = text.Substring(0, pos);
                    text = text.Replace(" ", "");

                    int price = int.Parse(text);
                    //Log.Info(tt.Text + " vs " + text + " value = " + price);


                    Assert.That(price >= min && price <= max);
                }
                var messageError = Driver.FindElements(By.ClassName("schema-products__message")).Any();
                if (messageError)
                    break;
                

                var button = Driver.FindElements(By.ClassName("schema-pagination__main")).FirstOrDefault();
                if (button != null)
                {
                    var tt = button.GetAttribute("class");
                    Log.Info("class = " + tt);
                    //if (tt.Contains("schema-pagination__main_disabled"))
                     //   break;
                    
                    button.Click();
                    Thread.Sleep(1500);
                }
                else break;
            }
        }

        public override void Finish()
        {
            Thread.Sleep(3000);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Quit();
        }

       
    }
}
