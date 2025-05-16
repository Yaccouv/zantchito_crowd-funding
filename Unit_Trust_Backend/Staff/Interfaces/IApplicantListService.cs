using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Staff.Interfaces
{
    public interface IApplicantListService
    {
        Task<List<Investors>> GetApplicantListAsync();
    }
}
