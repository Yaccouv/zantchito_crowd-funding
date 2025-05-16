using Unit_Trust_Backend.Investor.DTOs;

namespace Unit_Trust_Backend.Investor.Services
{
    public interface IPaymentService
    {
        Task<PaymentDTO> SavePaymentAsync(PaymentDTO paymentDto);
    }
}
