using OpenQA.Selenium;

namespace Test.UI.Web.Features.Pages.User
{
    public class Register : PageBase, IFormPage
    {
        public IWebElement Form
        {
            get { return Driver.FindElement(By.CssSelector("form[action='/user/register']")); }
        }

        public IWebElement Email
        {
            get { return Driver.FindElement(By.Id("Email")); }
        }

        public IWebElement Name
        {
            get { return Driver.FindElement(By.Id("Name")); }
        }

        public IWebElement Password
        {
            get { return Driver.FindElement(By.Id("Password")); }
        }

        public Register(IWebDriver driver) : base(driver)
        {
            Url = "http://localhost:50522/user/register";
        }

        public void Submit(params string[] args)
        {
            Email.SendKeys(args[0]);
            Name.SendKeys(args[1]);
            Password.SendKeys(args[2]);
            Form.Submit();
        }
    }
}
