using Unit_Trust_Backend.DTOs;

namespace Unit_Trust_Backend.Investor.Interfaces
{
    public interface IDepositService
    {
        Task<bool> DepositAsync(DepositDTO depositDTO);
    }
}
