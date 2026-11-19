using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare("Password",ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
