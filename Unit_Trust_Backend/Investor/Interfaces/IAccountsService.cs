using Unit_Trust_Backend.DTOs;

namespace Unit_Trust_Backend.Investor.Interfaces
{
    public interface IAccountsService
    {
        Task GenerateInvestorNumberAsync(int investorNumber);
    }
}
