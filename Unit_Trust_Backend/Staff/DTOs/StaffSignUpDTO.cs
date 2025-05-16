namespace Unit_Trust_Backend.Staff.DTOs
{
    public class StaffSignUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        // Employment Details
        public string JobTitle { get; set; }
        public string Department { get; set; }
    }
}
