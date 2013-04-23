using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Core.Domain.Model;
using Users = Core.Domain.Model.Users;
using Moq;
using NUnit.Framework;
using Presentation.Web.Validation.User;

namespace Test.Unit.Presentation.Web.Validation.User
{
    [TestFixture]
    public class UniqueEmailAttributeTest
    {
        protected UniqueEmailAttribute Attr { get; set; }

        protected Mock<IRepository<Users.User>> Repo { get; set; }

        [SetUp]
        public void SetUp()
        {
            Repo = new Mock<IRepository<Users.User>>();
            Attr = new UniqueEmailAttribute { Repo = Repo.Object };
        }

        [Test]
        public void ParameterlessConstructorShouldHaveDefaultMessage()
        {
            Assert.IsNotNull(GetMessagePropertyValue());
        }

        [Test]
        public void MessageInConstructorShouldSetMessage()
        {
            Attr = new UniqueEmailAttribute("testing");
            Assert.AreEqual("testing", GetMessagePropertyValue());
        }

        [Test]
        public void GetValidationResultShouldReturnNullWhenStringIsNull()
        {
            Assert.IsNull(InvokeGetValidationResult(new object[] { null }));
        }

        [Test]
        public void GetValidationResultWithFoundEmailShouldReturnValidationResult()
        {
            UserIsFound();
            var result = InvokeGetValidationResult(new object[] { "string" });
            Assert.IsInstanceOf<ValidationResult>(result);
        }

        [Test]
        public void GetValidationResultWhereEmailNotFoundReturnsNull()
        {
            UserIsNotFound();
            var result = InvokeGetValidationResult(new object[] { "string" });
            Assert.IsNull(result);
        }

        [Test]
        public void IsValidShouldReturnNullWhenValueIsNull()
        {
            var result = InvokeIsValid(null);
            Assert.IsNull(result);
        }

        [Test]
        public void IsValidShouldReturnValidationResultWhenUserFound()
        {
            UserIsFound();
            var result = InvokeIsValid("user");
            Assert.IsInstanceOf<ValidationResult>(result);
        }

        [Test]
        public void IsValidShouldReturnNullIfUserNotFound()
        {
            UserIsNotFound();
            var result = InvokeIsValid("user");
            Assert.IsNull(result);
        }

        protected void UserIsFound()
        {
            Repo.Setup(u => u.FindOneBy(It.IsAny<Func<Users.User, bool>>())).Returns(new Users.User());
        }

        protected void UserIsNotFound()
        {
            Repo.Setup(u => u.FindOneBy(It.IsAny<Func<Users.User, bool>>())).Returns<Users.User>(null);
        }

        protected ValidationResult InvokeIsValid(string value)
        {
            var method = Attr.GetType().GetMethod("IsValid", BindingFlags.Instance | BindingFlags.NonPublic);
            var valContext = new ValidationContext(this, null, null);
            var args = new object[] { value, valContext };
            return method.Invoke(Attr, args) as ValidationResult;
        }

        protected ValidationResult InvokeGetValidationResult(object[] args)
        {
            var method = Attr.GetType().GetMethod("GetValidationResult", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = method.Invoke(Attr, args);
            return result as ValidationResult;
        }

        protected string GetMessagePropertyValue()
        {
            var msgProp = Attr.GetType().GetProperty("Message", BindingFlags.NonPublic | BindingFlags.Instance);
            var msg = msgProp.GetValue(Attr, null);
            return (string)msg;
        }
    }
}