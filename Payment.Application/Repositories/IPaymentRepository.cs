using System;
namespace Payment.Application.Repositories
{
	public interface IPaymentRepository
	{
        Task Create(Payment.Domain.Entities.Payment payment);
        Task<Payment.Domain.Entities.Payment> Update(Payment.Domain.Entities.Payment payment);
        Task<Payment.Domain.Entities.Payment> GetPaymentByPaymentReference(string paymentReference);
    }
}

