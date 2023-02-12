using System;
using Payment.Application.Business;
using Payment.Application.Interfaces.Bank;
using Payment.Domain.Entities;

namespace Payment.Banks.BankAPIClients
{
	public class MockBankSimulator : IBankClient
    {

        public async Task<BankResponse> ProcessPayment(BankRequest bankRequest)
        {
            var status = PaymentStatus.Success;
            var message = string.Empty;
            switch (bankRequest.CardNumber)
            {
                case "5134123412341234":
                    status = PaymentStatus.Failure;
                    message = "Insufficient Funds";
                    break;
                    // ......
            }

            return await Task.FromResult(
                    new BankResponse
                    {
                        Reference = GeneratePaymentReference(),
                        Status = status,
                        Message = message
                    });
        }

        private string GeneratePaymentReference()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
        }
    }
}

