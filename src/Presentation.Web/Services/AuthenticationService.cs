using System;
using System.Web;
using System.Web.Security;
using Core.Domain.Model.Users;

namespace Presentation.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Authenticate(User user, HttpResponseBase response)
        {
            var ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), DateTime.Now, DateTime.Now.AddDays(4), false, string.Empty);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encrypted));
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}