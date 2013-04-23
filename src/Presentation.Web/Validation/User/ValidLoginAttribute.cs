using System.ComponentModel.DataAnnotations;
using Core.Domain.Model;
using CoreUsers = Core.Domain.Model.Users;
using Ninject;
using Presentation.Web.Models.Input;

namespace Presentation.Web.Validation.User
{
    public class ValidLoginAttribute : UserValidationAttributeBase
    {
        [Inject]
        public override IRepository<CoreUsers.User> Repo { get; set; }

        public ValidLoginAttribute(string message = "") : base(message)
        {
            
        }

        public ValidLoginAttribute()
            : this("Invalid Email or Password.")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return GetValidationResult((string)value, validationContext);
        }

        protected ValidationResult GetValidationResult(string email, ValidationContext context)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var user = Repo.FindOneBy(u => u.Email == email);
            var input = context.ObjectInstance as LoginInput;
            if(user == null || ! user.IsAuthenticated(input.Password))
                return new ValidationResult(Message);

            return null;
        }

        
    }
}