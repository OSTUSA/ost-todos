using System.Web;
using Core.Domain.Model.Users;

namespace Presentation.Web.Services
{
    public interface IAuthenticationService
    {
        void Authenticate(User user, HttpResponseBase response);
        void SignOut();
    }
}
