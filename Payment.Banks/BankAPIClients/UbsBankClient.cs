using System;
using Payment.Application.Business;
using Payment.Application.Interfaces.Bank;

namespace Payment.Banks.BankAPIClients
{
	public class UbsBankClient : IBankClient
    {
        public Task<BankResponse> ProcessPayment(BankRequest bankRequest)
        {
            //Actual HTTP request call to real bank call comes here.
            throw new NotImplementedException();
        }
    }
}

