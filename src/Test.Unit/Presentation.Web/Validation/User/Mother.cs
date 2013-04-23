using Users = Core.Domain.Model.Users;
using Presentation.Web.Models.Input;

namespace Test.Unit.Presentation.Web.Validation.User
{
    public static class Mother
    {
        public static LoginInput ValidLogin
        {
            get { return new LoginInput() {Email = "scaturrob@gmail.com", Password = "password"}; }
        }

        public static LoginInput BadLogin
        {
            get { return new LoginInput() { Email = "scaturrob@gmail.com", Password = "mismatch" }; }
        }

        public static Users.User ValidUser
        {
            get
            {
                var user = new Users.User() { Email = "scaturrob@gmail.com", Password = "password" };
                user.HashPassword();
                return user;
            }
        }
    }
}
