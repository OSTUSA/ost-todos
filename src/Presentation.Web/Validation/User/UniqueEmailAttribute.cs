using System.ComponentModel.DataAnnotations;
using Core.Domain.Model;
using CoreUsers = Core.Domain.Model.Users;
using Ninject;

namespace Presentation.Web.Validation.User
{
    public class UniqueEmailAttribute : UserValidationAttributeBase
    {
        [Inject]
        public override IRepository<CoreUsers.User> Repo { get; set; }

        public UniqueEmailAttribute(string message = "") : base(message)
        {
            
        }

        public UniqueEmailAttribute() : 
            this("This email address is already in use.")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return GetValidationResult((string)value);
        }

        protected ValidationResult GetValidationResult(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var user = Repo.FindOneBy(u => u.Email == email);
            if (user != null)
                return new ValidationResult(Message);

            return null;
        }
    }
}