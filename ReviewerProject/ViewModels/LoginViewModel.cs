using System.ComponentModel.DataAnnotations;

namespace ReviewerProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Empty email")]
        [MaxLength(80)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}
