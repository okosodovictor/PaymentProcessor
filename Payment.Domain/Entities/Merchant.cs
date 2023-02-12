using System;
namespace Payment.Domain.Entities
{
	public class Merchant
	{
        public Merchant()
        {
            Payments = new HashSet<Payment>();
        }

        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string Description { get; set; }
        public string AcquirerBank { get; set; } //BNF, 
        public string MerchantIdentificationNumber { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

