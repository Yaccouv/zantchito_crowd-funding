using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.DTOs
{
    public class UserSignInDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
