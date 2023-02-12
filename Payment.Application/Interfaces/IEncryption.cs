using System;
namespace Payment.Application.Interfaces
{
	public interface IEncryption
	{
        string Encrypt(string CardNumber);
        string Decrypt(string cardNumber);
        string Mask(string cardNumber);
    }
}

