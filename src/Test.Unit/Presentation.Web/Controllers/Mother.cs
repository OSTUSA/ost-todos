using Core.Domain.Model.Users;
using Presentation.Web.Models.Input;

namespace Test.Unit.Presentation.Web.Controllers
{
    public static class Mother
    {
        public static LoginInput ValidLogin
        {
            get { return new LoginInput() {Email = "scaturrob@gmail.com", Password = "password"}; }
        }

        public static LoginInput BadLogin
        {
            get { return new LoginInput() {Email = "m@e.c", Password = "mismatch"}; }
        }

        public static User ValidUser
        {
            get
            {
                var user = new User() {Email = "scaturrob@gmail.com", Password = "password"};
                user.HashPassword();
                return user;
            }
        }
    }
}
