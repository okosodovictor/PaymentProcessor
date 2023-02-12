using System;
using Payment.Application.Repositories;
using Payment.Domain.Entities;
using Payment.Persistence.EFConfiguration;

namespace Payment.Persistence.Repository
{
	public class MerchantRepository : IMerchantRepository
	{
        private readonly PaymentDbContext _context;

        public MerchantRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Merchant> GetMerchantById(Guid merchantId)
        {
            return await _context.Merchants.FindAsync(merchantId);
        }
    }
}

