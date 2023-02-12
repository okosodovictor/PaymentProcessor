using System;
namespace Payment.Api.Models
{
	public class MerchantDto
	{
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string Description { get; set; }
        public string AcquirerBank { get; set; }
    }
}

