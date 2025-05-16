using Microsoft.Identity.Client;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly MyDatabase _myDatabase;


        public AccountsService(MyDatabase myDatabase)
        {
            _myDatabase = myDatabase;
        }



        public async Task GenerateInvestorNumberAsync(int investorNumber)
        {

            var makeAccounts = new Accounts
            {
                InvestorNumber = investorNumber,
                AccountNumber = GenerateAccountNumber(investorNumber),
                Balance = 0.00,

            };
            _myDatabase.Accounts.Add(makeAccounts);
            await _myDatabase.SaveChangesAsync();
        }


        public long GenerateAccountNumber(int investorNumber)
        {
            Random random = new Random();
            int prex = 91000;
            int InvN = investorNumber;
            int sufx = random.Next(10000, 100000);
            string AccNumberStr = $"{prex}{InvN}{sufx}";
            long AccNumber = long.Parse(AccNumberStr);
            return AccNumber;
        }
    }
}
