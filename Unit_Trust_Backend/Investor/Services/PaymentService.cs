using System;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Investor.DTOs;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Investor.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MyDatabase _context;

        public PaymentService(MyDatabase context)
        {
            _context = context;
        }

        public async Task<PaymentDTO> SavePaymentAsync(PaymentDTO paymentDto)
        {
            var payment = new Payment
            {
                CampaignId = paymentDto.CampaignId,
                Amount = paymentDto.Amount,
                Status = paymentDto.Status,
                TransactionDate = paymentDto.TransactionDate
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            paymentDto.PaymentId = payment.PaymentId;
            return paymentDto;
        }
    }
}
