using System.ComponentModel.DataAnnotations;
using Presentation.Web.Validation.User;

namespace Presentation.Web.Models.Input
{
    public class RegisterInput
    {
        [Required, UniqueEmail]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}