using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Payment.Domain.Entities;
using Payment.Persistence.Configurations;

namespace Payment.Persistence.EFConfiguration
{
	public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment.Domain.Entities.Payment> Payments { get; set; }
        public DbSet<Merchant> Merchants { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Payment.Domain.Entities.Payment>()
                   .Property(p => p.PaymentId)
                   .HasColumnName("PaymentId")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Entity<Payment.Domain.Entities.Payment>()
                   .Property(p => p.CardHolderName)
                   .HasMaxLength(60);

            builder.Entity<Payment.Domain.Entities.Payment>()
                   .Property(p => p.CardNumber)
                   .HasMaxLength(16);

            builder.Entity<Payment.Domain.Entities.Payment>()
                   .Property(p => p.Currency)
                   .HasMaxLength(3);

            builder.Entity<Payment.Domain.Entities.Payment>()
                   .Property(p => p.Cvv)
                   .HasMaxLength(3);

            builder.Entity<Payment.Domain.Entities.Payment>()
                  .Property(p => p.ExpiryMonth)
                  .HasMaxLength(2);

            builder.Entity<Payment.Domain.Entities.Payment>()
                  .Property(p => p.ExpiryYear)
                  .HasMaxLength(2);

            builder.Entity<Merchant>()
                  .Property(p => p.MerchantId)
                  .HasColumnName("MerchantId")
                  .HasDefaultValueSql("gen_random_uuid()");

            builder.Entity<Payment.Domain.Entities.Payment>()
                   .HasOne(s => s.Merchant)
                   .WithMany(g => g.Payments)
                   .HasForeignKey(s => s.MerchantId);

            builder.ApplyConfiguration(new MerchantConfiguration());
        }
    }

    public class PaymentDbContextContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;port=5433;Database=PaymentDB;Username=postgres;Password=redmond");
            return new PaymentDbContext(optionsBuilder.Options);
        }
    }
}

