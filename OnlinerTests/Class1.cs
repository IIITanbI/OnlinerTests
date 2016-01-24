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
    public class Class1 : TestBase
    {
        public const string BaseUrl = "http://www.onliner.by";
        [Test]
        public void Start()
        {
            int min = 10000000;
            int max = 100000000;

            //Init();
            Catalog();
            ComputersAndNetworks();
            Computers();

            SetMinPrice(min);
            SetMaxPrice(max);
            Check(min, max);
            // Finish();
        }

        [Test]
        public override void Init()
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Driver.Navigate().GoToUrl(BaseUrl);
        }

        [Test]
        public void Catalog()
        {
            Driver.FindElement(By.XPath(".//*[@id='container']/div/div[2]/header/nav/ul[2]/li[1]/a")).Click();
        }
        [Test]
        public void ComputersAndNetworks()
        {
            Driver.FindElement(By.XPath(".//*[@id='container']/div/div[2]/div/div/div[1]/ul/li[3]/span[1]")).Click();
        }
        [Test]
        public void Computers()
        {
            Driver.FindElement(By.XPath(".//*[@id='container']/div/div[2]/div/div/div[1]/div[2]/div/div[2]/div[2]/ul/li[1]/span/a")).Click();
        }

        [Test]
        public void SetMinPrice(int price)
        {
            Driver.FindElement(By.XPath(".//*[@id='schema-filter']/div[1]/div[2]/div[2]/div/div[1]/input")).SendKeys(price.ToString());
        }
        [Test]
        public void SetMaxPrice(int price)
        {
            Driver.FindElement(By.XPath(".//*[@id='schema-filter']/div[1]/div[2]/div[2]/div/div[2]/input")).SendKeys(price.ToString());
        }


        [Test]
        public void Check(int min, int max)
        {
            Thread.Sleep(3000);
           //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
           //var elements = wait.Until(driver => driver.FindElements(By.ClassName("schema-product__price")));
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
                Log.Info(tt.Text  + " vs "  + text + " value = "  + price);

                if (price < min || price > max)
                {
                    Assert.IsTrue(false);
                }
            }
            
        }
        [Test]
        public override void Finish()
        {
            Thread.Sleep(3000);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Quit();
        }

       
    }
}
