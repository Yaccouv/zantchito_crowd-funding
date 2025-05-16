using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Investor.Interfaces
{
    public interface IInvestorService
    {
        Task<bool> CreateInvestorAsync(InvestorDTO createAccountDTO);
        Task GetUserId(int  userId);
        Task<List<Investors>> GetInvestmentsAsync();
    }
}
