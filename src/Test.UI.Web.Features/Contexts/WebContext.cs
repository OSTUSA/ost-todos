using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using Infrastructure.NHibernate.Mapping.Users;
using NHibernate;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using Test.Integration;
using Test.UI.Web.Features.Pages;

namespace Test.UI.Web.Features.Contexts
{
    /// <summary>
    /// A base WebContext for use in all features
    /// Here are places to initialize pages, submit forms,
    /// and test for cookies
    /// </summary>
    [Binding]
    public class WebContext
    {
        protected IWebDriver Driver;
        protected string BaseUrl;
        protected dynamic Page;
        protected static ISessionFactory SessionFactory;

        [BeforeFeature()]
        public static void InitFeature()
        {
            SessionFactory = new DatabaseTestState("TestConnection").Configure<UserMap>("Development");
        }

        [BeforeScenario()]
        public void InitScenario()
        {
            Driver = new FirefoxDriver();
            BaseUrl = "http://localhost:50522";
        }

        [AfterScenario()]
        public void TearDownScenario()
        {
            var authCookie = Driver.Manage().Cookies.GetCookieNamed(".ASPXAUTH");
            if(authCookie != null)
                Driver.Manage().Cookies.DeleteCookie(authCookie);
            Driver.Quit();
        }

        [Given(@"I am on page ""(.*)""")]
        [When(@"I am on page ""(.*)""")]
        public void GivenIAmOnPage(string relativeType)
        {
            var typeName = String.Format("Test.UI.Web.Features.Pages.{0}", relativeType);
            var type = Type.GetType(typeName);
            Object[] args = {Driver};
            Page = Activator.CreateInstance(type, args) as IPage;
            Page.NavigateTo();
        }

        [When(@"I submit the form using")]
        public void WhenISubmitTheFormUsing(Table table)
        {
            Page.Submit((from row in table.Rows from kvp in row select kvp.Value).ToArray());
        }

        [Then(@"A cookie named ""(.*)"" should exist")]
        public void ThenACookieNamedShouldExist(string cookieName)
        {
            var cookie = Driver.Manage().Cookies.GetCookieNamed(cookieName);
            Assert.IsNotNull(cookie);
        }

        [Then(@"I should be redirected to ""(.*)""")]
        public void ThenIShouldBeRedirectedTo(string path)
        {
            Assert.AreEqual(BaseUrl + path, Driver.Url);
        }

        [Then(@"element ""(.*)"" should have text")]
        public void ThenElementShouldHaveText(string selector)
        {
            var elem = Driver.FindElement(By.CssSelector(selector));
            Assert.IsNotNull(elem);
            Assert.IsNotEmpty(elem.Text);
        }

        [Then(@"element ""(.*)"" should be visible")]
        public void ThenElementShouldBeVisible(string selector)
        {
            var elem = Driver.FindElement(By.CssSelector(selector));
            Assert.IsNotNull(elem);
            Assert.True(elem.Displayed);
        }

        [Then(@"element ""(.*)"" should not exist")]
        public void ThenElementShouldNotExist(string selector)
        {
            Assert.Catch<NoSuchElementException>(() => Driver.FindElement(By.CssSelector(selector)));
        }

        [Then(@"I click element ""(.*)""")]
        public void ThenIClickElement(string selector)
        {
            var elem = Driver.FindElement(By.CssSelector(selector));
            elem.Click();
        }

        [Then(@"there should be no cookie named ""(.*)""")]
        public void ThenThereShouldBeNoCookieNamed(string cookieName)
        {
            var cookie = Driver.Manage().Cookies.GetCookieNamed(cookieName);
            Assert.IsNull(cookie);
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            var ticket = new FormsAuthenticationTicket(1, "testuser@email.com", DateTime.Now, DateTime.Now.AddDays(4), false, String.Empty);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new Cookie(FormsAuthentication.FormsCookieName, encrypted);
            Driver.Manage().Cookies.AddCookie(cookie);
        }

        [Given(@"I visit ""(.*)""")]
        [When(@"I visit ""(.*)""")]
        public void WhenIVisit(string path)
        {
            Driver.Navigate().GoToUrl(BaseUrl + path);
        }
    }
}
