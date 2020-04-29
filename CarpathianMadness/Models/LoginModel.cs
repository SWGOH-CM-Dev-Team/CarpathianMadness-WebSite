using System.ComponentModel.DataAnnotations;

namespace CarpathianMadness
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter your username!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }

    }

    public class ResetModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RegisterModel
    {

    }
}
