using Unit_Trust_Backend.Staff.DTOs;

namespace Unit_Trust_Backend.Staff.Interfaces
{
    public interface IStaffService
    {
        Task<bool> StaffSignUpAsync(StaffSignUpDTO staffSignUpDTO);
        Task<bool> StaffSignInAsync(StaffSignInDTO staffSignInDTO);  
    }
}
