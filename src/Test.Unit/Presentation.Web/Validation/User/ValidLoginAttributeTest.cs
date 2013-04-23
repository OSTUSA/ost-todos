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
    public class ValidLoginAttributeTest
    {
        protected ValidLoginAttribute Attr { get; set; }

        protected Mock<IRepository<Users.User>> Repo { get; set; }

        [SetUp]
        public void SetUp()
        {
            Repo = new Mock<IRepository<Users.User>>();
            Attr = new ValidLoginAttribute { Repo = Repo.Object };
        }

        [Test]
        public void ParameterlessConstructorShouldHaveDefaultMessage()
        {
            Assert.IsNotNull(GetMessagePropertyValue());
        }

        [Test]
        public void MessageInConstructorShouldSetMessage()
        {
            Attr = new ValidLoginAttribute("testing");
            Assert.AreEqual("testing", GetMessagePropertyValue());
        }

        [Test]
        public void GetValidationResultShouldReturnNullWhenStringIsNull()
        {
            var context = new ValidationContext(Mother.ValidLogin);
            Assert.IsNull(InvokeGetValidationResult(new object[] { null , context}));
        }

        [Test]
        public void GetValidationResultShouldReturnValidationResultIfRepoReturnsNull()
        {
            Func<Users.User, bool> pred = u => u.Email == Mother.ValidLogin.Email; 
            var context = new ValidationContext(Mother.ValidLogin);
            Repo.Setup(r => r.FindOneBy(pred)).Returns<Users.User>(null);
            var result = InvokeGetValidationResult(new object[] {Mother.ValidLogin.Email, context});
            Assert.IsInstanceOf<ValidationResult>(result);
        }

        [Test]
        public void GetValidationResultShouldReturnValidationResultIfInputPasswordDoesntMatchRepoUser()
        {
            Func<Users.User, bool> pred = u => u.Email == Mother.ValidUser.Email;
            var context = new ValidationContext(Mother.BadLogin);
            Repo.Setup(r => r.FindOneBy(pred)).Returns(Mother.ValidUser);
            var result = InvokeGetValidationResult(new object[] {Mother.BadLogin.Email, context});
            Assert.IsInstanceOf<ValidationResult>(result);
        }

        [Test]
        public void GetValidationResultShouldReturnNullIfAllIsValid()
        {
            var context = new ValidationContext(Mother.ValidLogin);
            Repo.Setup(r => r.FindOneBy(It.IsAny<Func<Users.User, bool>>())).Returns(Mother.ValidUser);
            var result = InvokeGetValidationResult(new object[] { Mother.ValidLogin.Email, context });
            Assert.IsNull(result);
        }

        protected string GetMessagePropertyValue()
        {
            var msgProp = Attr.GetType().GetProperty("Message", BindingFlags.NonPublic | BindingFlags.Instance);
            var msg = msgProp.GetValue(Attr, null);
            return (string)msg;
        }

        protected ValidationResult InvokeGetValidationResult(object[] args)
        {
            var method = Attr.GetType().GetMethod("GetValidationResult", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = method.Invoke(Attr, args);
            return result as ValidationResult;
        }
    }
}