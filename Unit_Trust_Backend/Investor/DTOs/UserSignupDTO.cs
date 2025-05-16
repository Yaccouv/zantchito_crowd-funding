using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.DTOs
{
    public class UserSignupDTO
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
