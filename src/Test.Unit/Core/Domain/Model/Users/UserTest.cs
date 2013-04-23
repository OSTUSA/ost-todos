using Core.Domain.Model.Users;
using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;

namespace Test.Unit.Core.Domain.Model.Users
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void HashPassword_should_hash_unhashed_password()
        {
            var user = new User() {Password = "password"};
            user.HashPassword();
            Assert.IsTrue(BCryptHelper.CheckPassword("password", user.Password));
        }

        [Test]
        public void IsAuthenticated_should_return_true_for_matching_unhashed_password()
        {
            var user = new User() {Password = "password"};
            user.HashPassword();
            Assert.True(user.IsAuthenticated("password"));
        }

        [Test]
        public void IsAuthenticated_should_return_fals_for_non_matching_unhashed_password()
        {
            var user = new User() { Password = "password" };
            user.HashPassword();
            Assert.False(user.IsAuthenticated("false"));
        }
    }
}
