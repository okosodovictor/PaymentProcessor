using System;
using Payment.Application.Business;

namespace Payment.Application.Interfaces.Bank
{
	public interface IBankClient
	{
        Task<BankResponse> ProcessPayment(BankRequest bankRequest);
    }
}

