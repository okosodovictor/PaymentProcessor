using System;
namespace Payment.Api.Models
{
	public class PaymentDto
	{
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string Status { get; set; }
        public string ExpiringDate { get; set; }
        public MerchantDto merchant { get; set; }
    }
}

