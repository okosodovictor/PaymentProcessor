using System;
using Payment.Domain.Entities;

namespace Payment.Application.Business
{
	public class BankResponse
	{
        public string Reference { get; set; }
        public PaymentStatus Status { get; set; }
        public string Message { get; set; }
    }
}

