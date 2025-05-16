using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Staff.DTOs;
using Unit_Trust_Backend.Staff.Interfaces;
using Unit_Trust_Backend.Staff.Models;
using Microsoft.EntityFrameworkCore;
using Unit_Trust_Backend.DTOs;

namespace Unit_Trust_Backend.Staff.Services
{
    public class StaffService: IStaffService
    {
        private readonly MyDatabase _myDatabase;

        public StaffService(MyDatabase myDatabase)
        {
            _myDatabase = myDatabase;
        }

        public async Task<bool> StaffSignUpAsync(StaffSignUpDTO staffSignUpDTO)
        {
            if (_myDatabase.Staffs.Any(u => u.EmailAddress == staffSignUpDTO.EmailAddress))
                return false;

            int password = Password();
            string pass = Convert.ToString(password);

            var Staffs = new Staffs
            {
                FirstName = staffSignUpDTO.FirstName,
                LastName = staffSignUpDTO.LastName,
                Gender = staffSignUpDTO.Gender,
                DateOfBirth = staffSignUpDTO.DateOfBirth,
                Address = staffSignUpDTO.Address,
                PhoneNumber = staffSignUpDTO.PhoneNumber,
                EmailAddress = staffSignUpDTO.EmailAddress,
                Password = HashPassword(pass),
                JobTitle = staffSignUpDTO.JobTitle,
                Department = staffSignUpDTO.Department
            };


            SendMailPassword(staffSignUpDTO.EmailAddress,  pass);
            _myDatabase.Staffs.Add(Staffs);
            await _myDatabase.SaveChangesAsync();
            return true;

        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hash).Replace("_", "").ToLower();
            }
        }

        public int Password()
        {
            Random random = new Random();
            int password = random.Next(10000, 100000);
            return password;
        }

        public void SendMailPassword(string email, string password)
        {
            string senderEmail = "mungoshiyacc4@gmail.com";
            string senderPassword = "uuft tnof utzk mwzb";

            // SMTP server configuration
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            
            // Recipient email
            string recipientEmail = email;

            // Email message details
            string subject = "Staff Password";
            string body = $"Welcome Staff! Your Password is {password}";

            try
            {
         
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; 

            
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }


        public async Task<bool> StaffSignInAsync(StaffSignInDTO StaffsignInDTO)
        {
            var staff = await _myDatabase.Staffs
                .FirstOrDefaultAsync(u => u.EmailAddress == StaffsignInDTO.EmailAddress);
            if (staff == null || !VerifyPassword(StaffsignInDTO.Password, staff.Password))
            {
                return false;
            }
            return true;
        }

        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }
    }
}
