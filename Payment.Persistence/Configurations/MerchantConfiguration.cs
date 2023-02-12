using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.Entities;

namespace Payment.Persistence.Configurations
{
	public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasData(
                new Merchant
                {
                    MerchantId = Guid.NewGuid(),
                    MerchantName = "Apple",
                    AcquirerBank = "Deutsche",
                    Description = "Online shop for Mac",
                    MerchantIdentificationNumber = Guid.NewGuid().ToString(),
                },
                new Merchant
                {
                    MerchantId = Guid.NewGuid(),
                    MerchantName = "Amazon",
                    AcquirerBank = "UBS",
                    Description = "Online shop for all Items",
                    MerchantIdentificationNumber = Guid.NewGuid().ToString(),
                });
        }
    }
}

