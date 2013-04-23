using Core.Domain.Model.Users;

namespace Test.Integration.Infrastructure.NHibernate.Repositories.NHibernateRepositoryTest
{
    public static class Mother
    {
        public static User SimpleUser 
        { 
            get
            {
                var user = new User()
                {
                    Email = "b@s.com",
                    Name = "brian",
                    Password = "pass"
                };
                user.HashPassword();
                return user;
            }  
        }

        public static User AnotherSimpleUser
        {
            get
            {
                var user = new User()
                {
                    Email = "s@b.com",
                    Name = "scaturro",
                    Password = "pass"
                };
                user.HashPassword();
                return user;
            }
        }
    }
}