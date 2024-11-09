using System.ComponentModel.DataAnnotations;

namespace ReviewerProject.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Empty first name")]
        [MaxLength(80, ErrorMessage = "Maximum 80 character long")]
        [MinLength(5, ErrorMessage = "Minimum 5 characters long")]
        public string FirstName { get; set; }

        [MaxLength(80, ErrorMessage = "Maximum 80 character long")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Empty email")]
        [MaxLength(80, ErrorMessage = "Maximum 80 character long")]
        [MinLength(5, ErrorMessage = "Minimum 5 characters long")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,10}", ErrorMessage = "Wrong emal address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Empty password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm password")]
        [MinLength(5, ErrorMessage = "Minimum 6 characters long")]
        public string PasswordConfirm { get; set; }
    }
}
