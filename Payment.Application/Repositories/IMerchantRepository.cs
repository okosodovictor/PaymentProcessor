using System;
using Payment.Domain.Entities;

namespace Payment.Application.Repositories
{
	public interface IMerchantRepository
	{
        Task<Merchant> GetMerchantById(Guid merchantId);
    }
}

