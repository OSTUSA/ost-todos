using OpenQA.Selenium;

namespace Test.UI.Web.Features.Pages.User
{
    public class Login : PageBase, IFormPage
    {
        public IWebElement Form
        {
            get { return Driver.FindElement(By.CssSelector("form[action='/user/login']")); }
        }

        public IWebElement Email
        {
            get { return Driver.FindElement(By.Id("Email")); }
        }

        public IWebElement Password
        {
            get { return Driver.FindElement(By.Id("Password")); }
        }

        public Login(IWebDriver driver) : base(driver)
        {
            Url = "http://localhost:50522/user/login";
        }

        public void Submit(params string[] args)
        {
            Email.SendKeys(args[0]);
            Password.SendKeys(args[1]);
            Form.Submit();
        }
    }
}
