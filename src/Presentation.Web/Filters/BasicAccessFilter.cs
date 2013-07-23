using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Core.Domain.Model;
using Core.Domain.Model.Users;

namespace Presentation.Web.Filters
{
    public class BasicAccessFilter : AuthorizationFilterAttribute
    {
        private readonly IRepository<User> _repo; 

        public BasicAccessFilter(IRepository<User> repo)
        {
            _repo = repo;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (ShouldSkipAuth(actionContext)) return;
            var user = GetUser(actionContext);
            if (user == null)
            {
                var resp = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                resp.Headers.Add("WWW-Authenticate", @"Basic realm='osttodos'");
                actionContext.Response = resp;
                return;
            }
            SetPrincipal(user);
            base.OnAuthorization(actionContext);
        }

        public bool ShouldSkipAuth(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
               || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private User GetUser(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.Headers.Authorization == null) return null;
            var user = UnpackUser(request);
            return user;
        }

        private User UnpackUser(HttpRequestMessage request)
        {
            var decoded = GetBase64DecodedString(request.Headers.Authorization.Parameter).Split(':');
            string email = decoded[0], password = decoded[1];
            var user = _repo.FindOneBy(u => u.Email == email);
            if (!user.IsAuthenticated(password)) return null;
            return user;
        }

        private static string GetBase64DecodedString(string toDecode)
        {
            var data = System.Convert.FromBase64String(toDecode);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        private static void SetPrincipal(User user)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.Id.ToString()), null);
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = Thread.CurrentPrincipal;
            }
        }
    }
}