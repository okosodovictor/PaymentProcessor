using System;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Repositories;
using Payment.Persistence.EFConfiguration;

namespace Payment.Persistence.Repository
{
	public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _dbContext;
        public PaymentRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Payment.Domain.Entities.Payment payment)
        {
            if (payment != null)
            {
                _dbContext.Payments.Add(payment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Payment.Domain.Entities.Payment> GetPaymentByPaymentReference(string reference)
        {
            return await _dbContext.Payments
                    .Include(m => m.Merchant)
                    .FirstOrDefaultAsync(r => r.Reference == reference);
        }

        public async Task<Payment.Domain.Entities.Payment> Update(Payment.Domain.Entities.Payment payment)
        {
            _dbContext.Payments.Update(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }
    }
}

