using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.UI.Models.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email adress is incorrect")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone number is incorrect")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(18, 100, ErrorMessage = "Age must be at least 18.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Enter your location")]
        public string Location { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
