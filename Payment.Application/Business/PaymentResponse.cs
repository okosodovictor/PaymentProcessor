using System;
using Payment.Domain.Entities;

namespace Payment.Application.Business
{
	public class PaymentResponse
	{
        public string Reference { get; set; }
        public PaymentStatus Status { get; set; }
    }
}

