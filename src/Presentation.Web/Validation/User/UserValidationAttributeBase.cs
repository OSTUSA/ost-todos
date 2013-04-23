using System.ComponentModel.DataAnnotations;
using Core.Domain.Model;
using CoreUsers = Core.Domain.Model.Users;

namespace Presentation.Web.Validation.User
{
    abstract public class UserValidationAttributeBase : ValidationAttribute
    {
        /// <summary>
        /// Define as abstract and force implementation in
        /// child attributes. For some reason Ninject will
        /// act screwy if you have an [Ineject] attribute
        /// on the base class
        /// </summary>
        abstract public IRepository<CoreUsers.User> Repo { get; set; }

        protected string Message { get; set; }

        protected UserValidationAttributeBase(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
                Message = message;
        }
    }
}