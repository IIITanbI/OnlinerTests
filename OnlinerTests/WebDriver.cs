using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace OnlinerTests
{
    public enum BrowserType
    {
        Chrome,
        Firefox,
        IE
    }
    public static class WebDriver
    {
        private static IWebDriver webDriver;
        private static WebDriverWait webDriverWait;

        public static IWebDriver GetDriver(BrowserType browserType = BrowserType.Firefox)
        {
            
            if (webDriver == null)
                switch (browserType)
                {
                    case BrowserType.Chrome:
                        webDriver = new ChromeDriver();
                        break;
                    case BrowserType.Firefox:
                        webDriver = new FirefoxDriver();
                        break;
                    case BrowserType.IE:
                        webDriver = new InternetExplorerDriver();
                        break;
                    default:
                        break;
                }
            
            return webDriver;
        }

        public static WebDriverWait GetWait()
        {
            if (webDriverWait == null)
                webDriverWait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(5));
            return webDriverWait;
        }

        public static void WaitUntilElementIsVisible(IWebElement webElement)
        {
            GetWait().Until(d =>
            {
                if (webElement == null) return null;
                return webElement.Displayed ? webElement : null;
            });
        }

        public static void WaitUntilElementIsEnabled(IWebElement webElement)
        {
            GetWait().Until(d =>
            {
                if (webElement == null) return null;
                return webElement.Enabled ? webElement : null;
            });
        }
    }
}
