using System;
namespace Payment.Application.Business
{
	public class BankRequest
	{
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string ExpiryYear { get; set; }
        public string ExpiryMonth { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}

