using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Security.Cryptography;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Services
{
    public class InvestorService: IInvestorService
    {
        private readonly MyDatabase _myDatabase;
        private readonly IAccountsService _accountsService;
        public static int _Id;

        public InvestorService(MyDatabase myDatabase, IAccountsService accountsService)
        {
            _myDatabase = myDatabase;
            _accountsService = accountsService;
        }

        public async Task<bool> CreateInvestorAsync(InvestorDTO investorDTO)
        {
            int investorNumber = GenerateInvestorNumber();
            var AddInvestor = new Investors
            {
                // Personal Information
                Firstname = investorDTO.Firstname,
                Lastname = investorDTO.Lastname,
                Gender = investorDTO.Gender,
                DateOfBirth = investorDTO.DateOfBirth,
                Nationality = investorDTO.Nationality,
                IdNumber = investorDTO.IdNumber,

                // Contact Information
                Email = investorDTO.Email,
                PhoneNumber = investorDTO.PhoneNumber,
                City = investorDTO.City,

                // Financial Information
                EmploymentStatus = investorDTO.EmploymentStatus,
                AnnualIncome = investorDTO.AnnualIncome,
                SourceOfFunds = investorDTO.SourceOfFunds,
                BankAccountNumber = investorDTO.BankAccountNumber,
                BankBranchCode = investorDTO.BankBranchCode,
                BankAccountType = investorDTO.BankAccountType,

                // Investment Preferences
                InvestmentGoals = investorDTO.InvestmentGoals,
                PaymentPlan = investorDTO.PaymentPlan,

                // Beneficiary Information (Optional)
                NextOfKinName = investorDTO.NextOfKinName,
                NextOfKinRelationship = investorDTO.NextOfKinRelationship,
                NextOfKinPhone = investorDTO.NextOfKinPhone,
                Status = "Pending",
                InvestorNumber = investorNumber,
                UserId = _Id,
            };
            
            _myDatabase.Investors.Add(AddInvestor);
            await _myDatabase.SaveChangesAsync();
            await _accountsService.GenerateInvestorNumberAsync(investorNumber);


            return true;
        }

       public int GenerateInvestorNumber()
        {
            Random random = new Random();
            int InvNumber;
            do
            {
                InvNumber = random.Next(10000, 100000);

                bool exists = _myDatabase.Investors.Any(a => a.InvestorNumber == InvNumber);
                if (!exists)
                {
                    break;
                }
            }
            while (true);

            return InvNumber;

        }

        public Task GetUserId(int userId)
        {
            _Id = userId;
            return Task.CompletedTask;
        }


        public async Task <List<Investors>> GetInvestmentsAsync()
        {
            return await _myDatabase.Investors.Where(u => u.UserId == _Id).ToListAsync();
        }
    }
}
