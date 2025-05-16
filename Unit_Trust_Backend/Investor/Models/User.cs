using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.Investor.Models
{
    public class User
    {

        public int UserId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
