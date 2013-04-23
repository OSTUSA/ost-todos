using OpenQA.Selenium;

namespace Test.UI.Web.Features.Pages
{
    public interface IFormPage : IPage
    {
        IWebElement Form { get; }

        void Submit(params string[] args);
    }
}
