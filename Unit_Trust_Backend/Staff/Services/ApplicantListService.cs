using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Staff.Interfaces;

namespace Unit_Trust_Backend.Staff.Services
{
    public class ApplicantListService: IApplicantListService
    {
        private readonly MyDatabase _database;

        public ApplicantListService(MyDatabase database)
        {
            _database = database;
        }

        public async Task<List<Investors>> GetApplicantListAsync()
        {
            return await _database.Investors.Where(u => u.Status == "Pending").ToListAsync();
        }
    }
}
