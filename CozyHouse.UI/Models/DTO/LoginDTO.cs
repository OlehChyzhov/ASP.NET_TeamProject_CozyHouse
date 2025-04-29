using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.UI.Models.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string? UserEmail { get; set; }

        [Required]
        [DisplayName("Password")]
        public string? UserPassword { get; set; }
    }
}
