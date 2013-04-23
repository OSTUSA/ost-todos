using Core.Domain.Model.Users;
using FluentMigrator;

namespace Infrastructure.Migrations.Profiles
{
    [Profile("Development")]
    public class CreateDevelopmentUsers : Migration
    {
        public override void Down()
        {
            //not in use
        }

        public override void Up()
        {
            Insert.IntoTable("User").Row(new
            {
                Email = "test1@test.com",
                Name = "Test User1",
                Password = HashPassword("password")
            });
        }

        protected string HashPassword(string password)
        {
            var user = new User();
            user.Password = password;
            user.HashPassword();
            return user.Password;
        }
    }
}