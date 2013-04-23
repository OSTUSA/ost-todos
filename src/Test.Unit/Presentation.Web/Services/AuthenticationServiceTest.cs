using System.Web;
using Core.Domain.Model.Users;
using Moq;
using NUnit.Framework;
using Presentation.Web.Services;

namespace Test.Unit.Presentation.Web.Services
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        protected User User;

        protected AuthenticationService Service;

        [SetUp]
        public void Init()
        {
            User = new User() { Email = "e@a.com", Name = "Brian", Password = "password" };

            Service = new AuthenticationService();
        }

        [Test]
        public void Authenticate_should_set_cookie()
        {
            var response = new Mock<HttpResponseBase>();
            var collection = new HttpCookieCollection();
            response.SetupGet(x => x.Cookies).Returns(collection);
            Service.Authenticate(User, response.Object);
            Assert.AreEqual(1, collection.Count);
        }
    }
}
