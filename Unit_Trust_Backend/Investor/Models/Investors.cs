using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.Investor.Models
{
    public class Investors
    {
        //public int InvestorId { get; set; }
        [Key]
        public int InvestorNumber { get; set; }
        public int UserId { get; set; }

        // Personal Information
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Gender { get; set; } // Male, Female, Other

        [Required]
        public DateTime DateOfBirth { get; set; } // To calculate age

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string IdNumber { get; set; }

        // Contact Information
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }

        // Financial Information
        [Required]
        public string EmploymentStatus { get; set; } // Employed, Self-employed, Retired, etc.

        [Required]
        public decimal AnnualIncome { get; set; } // Annual income for assessment

        [Required]
        public string SourceOfFunds { get; set; } // Employment, Savings, Investments, etc.

        [Required]
        public string BankAccountNumber { get; set; }

        [Required]
        public string BankBranchCode { get; set; } // or Routing Number

        [Required]
        public string BankAccountType { get; set; } // Checking, Savings, etc.

        // Investment Preferences
        [Required]
        public string InvestmentGoals { get; set; } // Retirement, Wealth Accumulation, Education, etc.

        [Required]
        public string PaymentPlan { get; set; } // Lump Sum, Monthly Contributions, etc.

        // Beneficiary Information (Optional)
        public string NextOfKinName { get; set; }

        public string NextOfKinRelationship { get; set; }

        public string NextOfKinPhone { get; set; }

        public string Status { get; set; }
    }
}
