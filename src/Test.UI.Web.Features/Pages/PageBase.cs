using OpenQA.Selenium;

namespace Test.UI.Web.Features.Pages
{
    abstract public class PageBase : IPage
    {
        public string Url { get; set; }
        public IWebDriver Driver { get; set; }

        protected PageBase(IWebDriver driver)
        {
            Driver = driver;
        }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(Url);
        }
    }
}
