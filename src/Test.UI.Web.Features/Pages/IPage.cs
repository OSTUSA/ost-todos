using OpenQA.Selenium;

namespace Test.UI.Web.Features.Pages
{
    public interface IPage
    {
        IWebDriver Driver { get; set; }
        string Url { get; set; }

        void NavigateTo();
    }
}
