using System.Threading.Tasks;
using Unit_Trust_Backend.Investor.Models;
using Microsoft.EntityFrameworkCore;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.DTOs;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Unit_Trust_Backend.Investor.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly MyDatabase _database;
        public static int _Id;

        public CampaignService(MyDatabase database)
        {
            _database = database;
        }

        public async Task<bool> CreateCampaignAsync(CampaignDTO campaignDTO)
        {
            var campaign = new Campaign
            {
                BusinessName = campaignDTO.BusinessName,
                BusinessCategory = campaignDTO.BusinessCategory,
                ProjectTitle = campaignDTO.ProjectTitle,
                FundingGoal = campaignDTO.FundingGoal,
                CreatedAt = DateTime.UtcNow,
                BusinessDescription = campaignDTO.BusinessDescription,
                UserId = _Id, // Assuming _Id is the current user's ID
                Status = "0"  // Default status
            };

            // Custom folder path for images (change to your desired folder path)
            var customFolderPath = Path.Combine(@"C:\Users\T590\source\repos\Unit_Trust_Management_System\Unit_Trust_Backend\img");

            // Ensure the directory exists
            if (!Directory.Exists(customFolderPath))
            {
                Directory.CreateDirectory(customFolderPath);
            }

            // Handle Business Image (Upload)
            if (campaignDTO.BusinessImage != null && campaignDTO.BusinessImage.Length > 0)
            {
                var imagePath = Path.Combine(customFolderPath, campaignDTO.BusinessImage.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await campaignDTO.BusinessImage.CopyToAsync(stream);
                }
                // Store the relative path in DB (String)
                campaign.BusinessImage = "/img/" + campaignDTO.BusinessImage.FileName; // ✅ Correct: Save relative file path
            }

            // Handle ID Image (Upload)
            if (campaignDTO.IDImage != null && campaignDTO.IDImage.Length > 0)
            {
                var idImagePath = Path.Combine(customFolderPath, campaignDTO.IDImage.FileName);
                using (var stream = new FileStream(idImagePath, FileMode.Create))
                {
                    await campaignDTO.IDImage.CopyToAsync(stream);
                }
                // Store the relative path in DB (String)
                campaign.IDImage = "/img/" + campaignDTO.IDImage.FileName; // ✅ Correct: Save relative file path
            }

            // Add the campaign to the database
            _database.Campaign.Add(campaign);
            await _database.SaveChangesAsync();

            return true;
        }





        public async Task<List<Campaign>> GetCampaigns()
        {
            var campaigns = await _database.Campaign
                .Where(c => c.Status == "0")
                .ToListAsync();

            // Correct the file path concatenation to avoid double "/img/"
            foreach (var campaign in campaigns)
            {
                // Fix BusinessImage path
                if (!campaign.BusinessImage.StartsWith("/img/"))
                {
                    campaign.BusinessImage = "/img/" + campaign.BusinessImage;
                }

                // Fix IDImage path
                if (!campaign.IDImage.StartsWith("/img/"))
                {
                    campaign.IDImage = "/img/" + campaign.IDImage;
                }
            }

            return campaigns;
        }






        public Task GetUserId(int userId)
        {
            _Id = userId;
            return Task.CompletedTask;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _database.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }


        public async Task<bool> UpdateCampaignStatusAsync(int campaignId, string status)
        {
            var campaign = await _database.Campaign.FindAsync(campaignId);
            if (campaign == null) return false;

            // Get the userId associated with the campaign
            var userId = campaign.UserId; // Assuming the 'Campaign' table has a 'UserId' field that references the user

            // Get the user's information (FirstName, LastName, and Email) from the 'Users' table
            var user = await _database.Users.FindAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.Email)) return false;

            // Update the campaign's status and dates
            campaign.Status = status;


            var projectDTO = new ProjectDTO
            {
                CampaignId = campaignId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            };

            var project = new Project
            {
                CampaignId = projectDTO.CampaignId,
                StartDate = projectDTO.StartDate,
                EndDate = projectDTO.EndDate
            };

            // Add project to the database
            await _database.Project.AddAsync(project);

            // Save changes to both Campaign and Project tables
            await _database.SaveChangesAsync();

            // Compose the email message body
            var userName = $"{user.Firstname} {user.Lastname}";
            var emailBody = $"Dear {userName},\n\nCongratulations! Your business idea, has been selected for crowdfunding on Zatchito!\r\n\r\nOur team was impressed by the potential impact and innovation your idea brings. We believe it has the potential to make a meaningful difference, and we’re thrilled to help you bring it to life.\r\n\r\nHere’s what happens next:\r\n1. Your crowdfunding campaign is now live, and we’ll send you details on how to promote it to your network.\r\n2. Our team will be here to support you at every step, from campaign promotion to managing donations.\r\n3. As your campaign progresses, we’ll provide you with regular updates and insights on your funding status.\r\n\r\nYou can now visit [Zatchito](https://www.zatchito.com) to monitor your funds and track the progress of your campaign.\r\n\r\nBest regards,  \r\nYour Zatchito Campaign Team";

            // Send email
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mungoshiyacc4@gmail.com", "heaj kqyi rrsn vnub"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("mungoshiyacc4@gmail.com"),
                Subject = "Campaign Status Updated",
                Body = emailBody,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(user.Email);

            await smtpClient.SendMailAsync(mailMessage);

            return true;
        }






        /*public async Task<bool> UpdateCampaignStatusAsync(int campaignId, string status)
        {
            var campaign = await _database.Campaign.FindAsync(campaignId);
            if (campaign == null) return false;

            // Get the userId associated with the campaign
            var userId = campaign.UserId; // Assuming the 'Campaign' table has a 'UserId' field that references the user

            // Get the user's information (FirstName, LastName, and Email) from the 'Users' table
            var user = await _database.Users.FindAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.Email)) return false;

            // Update the campaign's status
            campaign.Status = status;
            campaign.StartDate = DateTime.UtcNow;
            campaign.EndDate = DateTime.UtcNow.AddDays(30);
            await _database.SaveChangesAsync();

            // Compose the email message body
            var userName = $"{user.Firstname} {user.Lastname}";
            var emailBody = $"Dear {userName},\n\nCongratulations! Your business idea, has been selected for crowdfunding on Zatchito!\r\n\r\nOur team was impressed by the potential impact and innovation your idea brings. We believe it has the potential to make a meaningful difference, and we’re thrilled to help you bring it to life.\r\n\r\nHere’s what happens next:\r\n1. Your crowdfunding campaign is now live, and we’ll send you details on how to promote it to your network.\r\n2. Our team will be here to support you at every step, from campaign promotion to managing donations.\r\n3. As your campaign progresses, we’ll provide you with regular updates and insights on your funding status.\r\n\r\nYou can now visit [Zatchito](https://www.zatchito.com) to monitor your funds and track the progress of your campaign.\r\n\r\nBest regards,  \r\nYour Zatchito Campaign Team";

            // Send email
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mungoshiyacc4@gmail.com", "heaj kqyi rrsn vnub"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("mungoshiyacc4@gmail.com"),
                Subject = "Campaign Status Updated",
                Body = emailBody,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(user.Email);

            await smtpClient.SendMailAsync(mailMessage);

            return true;
        }*/







        public async Task<List<object>> GetActiveCampaigns()
        {
            var campaigns = await _database.Campaign
                .Where(c => c.Status == "1") // Only active campaigns
                .ToListAsync();

            List<object> campaignWithDetails = new List<object>();

            foreach (var campaign in campaigns)
            {
                // Fix BusinessImage path
                if (!campaign.BusinessImage.StartsWith("/img/"))
                {
                    campaign.BusinessImage = "/img/" + campaign.BusinessImage;
                }

                // Fix IDImage path
                if (!campaign.IDImage.StartsWith("/img/"))
                {
                    campaign.IDImage = "/img/" + campaign.IDImage;
                }

                // Calculate RaisedAmount
                var raisedAmount = _database.Payments
                    .Where(p => p.CampaignId == campaign.CampaignId && p.Status == "succeeded")
                    .Sum(p => (decimal?)p.Amount) ?? 0;

                // Get associated project (if exists)
                var project = await _database.Project
                    .FirstOrDefaultAsync(p => p.CampaignId == campaign.CampaignId);

                // If no project is found, set default values for StartDate and EndDate
                DateTime? startDate = project?.StartDate;
                DateTime? endDate = project?.EndDate;

               

                // Create an anonymous object that combines the campaign data with RaisedAmount and project dates
                var campaignDataWithDetails = new
                {
                    campaign.CampaignId,
                    campaign.BusinessName,
                    campaign.BusinessImage,
                    campaign.IDImage,
                    campaign.BusinessCategory,
                    campaign.ProjectTitle,
                    campaign.FundingGoal,
                    StartDate = startDate,
                    EndDate = endDate,
                    campaign.BusinessDescription,
                    RaisedAmount = raisedAmount
                };

                campaignWithDetails.Add(campaignDataWithDetails);
            }

            return campaignWithDetails;
        }










    }
}
