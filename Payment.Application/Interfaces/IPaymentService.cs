using System;
using Payment.Application.Business;
using Payment.Domain.Entities;

namespace Payment.Application.Interfaces
{
	public interface IPaymentService
	{
        Task<PaymentResponse> RequestPayment(PaymentRequest model);
        Task<Payment.Domain.Entities.Payment> GetPaymentByReference(string reference);
    }
}

