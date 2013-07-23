using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Domain.Model;
using Core.Domain.Model.Users;
using Presentation.Web.Models.Display;
using Presentation.Web.Models.Input;

namespace Presentation.Web.Controllers
{
    public class UsersController : ApiController
    {
        protected IRepository<User> Users;

        public UsersController(IRepository<User> users)
        {
            Users = users;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Register(RegisterInput input)
        {
            if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.OK);

            var user = new User() {Email = input.Email, Name = input.Name, Password = input.Password};
            user.HashPassword();

            Users.Store(user);

            return Request.CreateResponse(HttpStatusCode.OK, new UserDisplay() { Email = user.Email, Name = user.Name, Id = user.Id});
        }
    }
}
