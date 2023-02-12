using System;
using Payment.Application.Business;
using Payment.Application.Interfaces;
using Payment.Application.Interfaces.Bank;
using Payment.Application.Repositories;
using Payment.Domain.Entities;
using Payment.Domain.Exceptions;

namespace Payment.Application.Managers.Services
{
	public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEncryption _encryption;
        private readonly IMerchantRepository _merchantRepo;
        private readonly IBankClient _bankClient;

        public PaymentService(IPaymentRepository paymentRepository,
            IMerchantRepository merchantRepo,
            IEncryption encryption, IBankClient bankClient)
        {
            _paymentRepository = paymentRepository;
            _encryption = encryption;
            _merchantRepo = merchantRepo;
            _bankClient = bankClient;
        }


        public async Task<PaymentResponse> RequestPayment(PaymentRequest model)
        {
            var result = model.Validate();
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                var errorMessage = string.Join(", ", errors);
                throw new ArgumentException("Payment Request is Invalid: " + errorMessage);
            }

            var merchant = await _merchantRepo.GetMerchantById(model.MerchantId.Value);
            if (merchant == null)
            {
                throw new NotFoundException("Invalid Merchant Id");
            }

            var payment = new Payment.Domain.Entities.Payment
            {
                CardNumber = _encryption.Mask(model.CardNumber),
                Cvv = model.CardCvv,
                CreatedOn = DateTime.UtcNow,
                CardHolderName = model.CardHolderName,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Amount = model.Amount,
                Currency = model.Currency,
                Status = PaymentStatus.Pending,
                MerchantId = merchant.MerchantId,
            };

            await _paymentRepository.Create(payment);
            //bank request
            var bankRequest = new BankRequest
            {
                CardNumber = model.CardNumber,
                CardCvv = model.CardCvv,
                CardHolderName = model.CardHolderName,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                Amount = model.Amount,
            };

            //Make call to bank
            var response = await _bankClient.ProcessPayment(bankRequest);
            payment.Reference = response.Reference;
            payment.Status = response.Status;

            await _paymentRepository.Update(payment);
            return new PaymentResponse
            {
                Status = payment.Status,
                Reference = payment.Reference
            };
        }

        public Task<Payment.Domain.Entities.Payment> GetPaymentByReference(string paymentReference)
        {
            return _paymentRepository.GetPaymentByPaymentReference(paymentReference);
        }
    }
}

