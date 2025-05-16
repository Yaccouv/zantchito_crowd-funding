using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Services
{
    public class DepositService: IDepositService
    {
        private MyDatabase _myDatabase;

        public DepositService(MyDatabase myDatabase)
        {
            _myDatabase = myDatabase;
        }

        public async Task<bool> DepositAsync(DepositDTO depositDTO)
        {
            //var deposit = new Deposit()
            //{
            //    Transactiondate = DateTime.Now,
            //    TransactionType = depositDTO.TransactionType,
            //    Amount = depositDTO.Amount,
            //}

            await _myDatabase.SaveChangesAsync();
            return true;
        }
    }
}
