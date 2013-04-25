using System.Web.Http;
using Core.Domain.Model;
using Core.Domain.Model.Users;
using Ninject;

namespace Presentation.Web.Controllers
{
    public class ControllerBase : ApiController
    {
        [Inject]
        public IRepository<User> UserRepo { get; set; }

        protected User LoadUser()
        {
            long id = 0;
            long.TryParse(User.Identity.Name, out id);
            return UserRepo.Load(id);
        }
    }
}