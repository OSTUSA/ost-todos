using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Core.Domain.Model;
using Core.Domain.Model.Users;
using Moq;
using NUnit.Framework;
using Presentation.Web.Controllers;
using Presentation.Web.Models.Input;
using Presentation.Web.Services;

namespace Test.Unit.Presentation.Web.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        protected UserController Controller;
        protected Mock<IAuthenticationService> Auth;
        protected Mock<IRepository<User>> Repo;
        protected Mock<HttpContextBase> Context;
        protected Mock<HttpResponseBase> Response;

        [SetUp]
        public void Init()
        {
            Repo = new Mock<IRepository<User>>();
            Auth = new Mock<IAuthenticationService>();
            Controller = new UserController(Repo.Object, Auth.Object);
            Context = new Mock<HttpContextBase>(MockBehavior.Strict);
            Response = new Mock<HttpResponseBase>();
            Response.SetupGet(x => x.Cookies).Returns(new HttpCookieCollection());
            Context.SetupGet(x => x.Response).Returns(Response.Object);
            Controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), Controller);
        }

        [Test]
        public void Register_should_return_a_ViewResult()
        {
            var result = Controller.Register();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Register_should_return_ViewResult_if_model_has_error()
        {
            Controller.ModelState.AddModelError("Email", "Email is required");
            var result = Controller.Register(new RegisterInput());
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Register_should_return_RedirectToRouteResult_if_input_is_valid()
        {
            var result =
                Controller.Register(new RegisterInput()
                    {
                        Email = "scaturrob@gmail.com",
                        Name = "Brian",
                        Password = "password"
                    });
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Register_with_valid_model_should_store_new_user()
        {
            var input = new RegisterInput() {Email = "m@e.com", Password = "p", Name = "n"};
            Controller.Register(input);
            Repo.Verify(x => x.Store(It.IsAny<User>()));
        }

        [Test]
        public void LogOut_should_call_signout_of_AuthenticationService_and_return_RedirectToActionResult()
        {
            var result = Controller.LogOut();
            Auth.Verify(a => a.SignOut());
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void LogIn_should_return_a_ViewResult()
        {
            var result = Controller.LogIn();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void LogIn_with_valid_model_should_call_Authentication_Authenticate_with_authed_User()
        {
            var tuple = DoLogin(Mother.ValidLogin, Mother.ValidUser);
            Auth.Verify(a => a.Authenticate(tuple.Item1, Response.Object));
        }

        [Test]
        public void LogIn_with_valid_model_should_return_RedirectToRouteResult()
        {
            var tuple = DoLogin(Mother.ValidLogin, Mother.ValidUser);
            Assert.IsInstanceOf<RedirectToRouteResult>(tuple.Item2);
        }

        [Test]
        public void LogIn_with_invalid_model_should_return_view_result()
        {
            Controller.ModelState.AddModelError("Email", "Invalid email");
            var result = DoLogin(Mother.ValidLogin, Mother.ValidUser);
            Assert.IsInstanceOf<ViewResult>(result.Item2);
        }

        private Tuple<User, ActionResult> DoLogin(LoginInput input, User returnUser)
        {
            Repo.Setup(r => r.FindOneBy(It.IsAny<Func<User, bool>>())).Returns(returnUser);
            return new Tuple<User, ActionResult>(returnUser, Controller.LogIn(input));
        }
    }
}
