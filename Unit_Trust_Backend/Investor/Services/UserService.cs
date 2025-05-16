using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Investor.Services;
using Unit_Trust_Backend.Staff.DTOs;
using Unit_Trust_Backend.Staff.Interfaces;
using Unit_Trust_Backend.Staff.Services;

namespace Unit_Trust_Backend.Services
{
    public class UserService : IUserService
    {

        private readonly MyDatabase _database;
        private readonly IInvestorService _investorService;
        private readonly IStaffService _staffService;
        private readonly IConfiguration _configuration;
        private readonly ICampaignService _campaignService;

        public UserService(MyDatabase database,ICampaignService campaignService, IInvestorService investorService, IStaffService staffService, IConfiguration configuration)
        {
            _database = database;
            _investorService = investorService;
            _staffService = staffService;
            _configuration = configuration;
            _campaignService = campaignService;
        }

        public async Task<bool> RegisterUserAsync(UserSignupDTO signupDTO)
        {
            if (_database.Users.Any(u => u.Email == signupDTO.Email))
                return false;

            var user = new User
            {
                Email = signupDTO.Email,
                PasswordHash = HashPassword(signupDTO.Password),
                Firstname = signupDTO.Firstname,
                Lastname = signupDTO.Lastname
            };

            _database.Users.Add(user);
            await _database.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("_", "").ToLower();
            }
        }


        public async Task<(bool IsSuccessfull, string UserType, string Token)> LoginUserAsync(UserSignInDTO signInDTO)
        {
            string domain = signInDTO.Email.Split('@')[1].ToLower();
            if (domain != "unittrust.org") // Investor login
            {
                var user = await _database.Users.FirstOrDefaultAsync(u => u.Email == signInDTO.Email);
                if (user == null || !VerifyPassword(signInDTO.Password, user.PasswordHash))
                {
                    return (false, null, null); // Invalid login
                }

                var token = GenerateJWToken(user); // Generate JWT for user

                // Assuming GetUserId is some necessary service call for Investors
                var userID = user.UserId;
                await _investorService.GetUserId(userID);
                await _campaignService.GetUserId(userID);

                return (true, "Investor", token); // Return JWT token
            }
            else // Staff login
            {
                var myStaff = new StaffSignInDTO
                {
                    EmailAddress = signInDTO.Email,
                    Password = signInDTO.Password
                };

                var stafflogged = await _staffService.StaffSignInAsync(myStaff);
                if (!stafflogged)
                {
                    return (false, null, null); // Invalid login
                }

                // Generate JWT for staff
                var token = GenerateJWTokenForStaff(myStaff);
                return (true, "Staff", token); // Return JWT token for staff
            }
        }

        // Verify Password method remains the same
        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }

        // Generate JWT for User
        private string GenerateJWToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),    // User email
        new Claim("UserId", user.UserId.ToString()),           // User ID for backend operations
        new Claim("Role", "Investor")                          // Role to identify the user type
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Adjust token expiration as necessary
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate JWT for Staff (if needed)
        private string GenerateJWTokenForStaff(StaffSignInDTO staff)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, staff.EmailAddress),  // Staff email
        new Claim("Role", "Staff")                                    // Role to identify the user type
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Adjust token expiration as necessary
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
