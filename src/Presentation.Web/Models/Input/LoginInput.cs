using System.ComponentModel.DataAnnotations;
using Presentation.Web.Validation.User;

namespace Presentation.Web.Models.Input
{
    public class LoginInput
    {
        [Required, ValidLogin]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}