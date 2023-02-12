using Moq;
using Payment.Application.Business;
using Payment.Application.Interfaces;
using Payment.Application.Interfaces.Bank;
using Payment.Application.Managers.Services;
using Payment.Application.Repositories;
using Payment.Domain.Entities;
using Payment.Domain.Exceptions;

namespace Payment.Test;

[TestClass]
public class PaymentServiceTest
{
    private Mock<IPaymentRepository> _paymentRepository;
    private Mock<IEncryption> _encryption;
    private Mock<IMerchantRepository> _merchantRepo;
    private Mock<IBankClient> _bankClient;

    [TestInitialize()]
    public void Startup()
    {
        _paymentRepository = new Mock<IPaymentRepository>();
        _encryption = new Mock<IEncryption>();
        _merchantRepo = new Mock<IMerchantRepository>();
        _bankClient = new Mock<IBankClient>();
    }

    [TestMethod]
    public async Task GetPaymentByReference_WithValidReference_ShouldPass()
    {
        //arrange
        var reference = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
        _paymentRepository.Setup(x => x.GetPaymentByPaymentReference(It.IsAny<string>())).Returns(Task.FromResult(new Payment.Domain.Entities.Payment
        {
            Reference = reference,
            Amount = 2000
        }));

        //Act
        var payService = new PaymentService(_paymentRepository.Object, _merchantRepo.Object, _encryption.Object, _bankClient.Object);
        var response = await payService.GetPaymentByReference(reference);

        //Assert
        Assert.AreEqual(reference, response.Reference);
        _paymentRepository.Verify(repo => repo.GetPaymentByPaymentReference(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task RequestPayment_WithNoMerchantId_ShouldFail()
    {
        //arrange
        var paymentRequest = new PaymentRequest
        {
            CardHolderName = "John Doe",
            CardNumber = "1234567890234567",
            CardCvv = "345",
            ExpiryYear = "24",
            Currency = "USD",
            Amount = 3000
        };

        //Act
        var payService = new PaymentService(_paymentRepository.Object, _merchantRepo.Object, _encryption.Object, _bankClient.Object);
        //Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => payService.RequestPayment(paymentRequest));
    }

    [TestMethod]
    public async Task RequestPayment_WithInvalidMerchantId_ShouldFail()
    {
        //arrange
        var merchantId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            MerchantId = merchantId,
            CardHolderName = "John Doe",
            CardNumber = "1234567890234567",
            CardCvv = "345",
            ExpiryYear = "24",
            ExpiryMonth = "04",
            Currency = "USD",
            Amount = 3000
        };

        //Act
        var payService = new PaymentService(_paymentRepository.Object, _merchantRepo.Object, _encryption.Object, _bankClient.Object);
        //Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(() => payService.RequestPayment(paymentRequest));
    }

    [TestMethod]
    public async Task RequestPayment_WithValidRequest_ShouldPass()
    {
        //arrange
        var merchantId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            MerchantId = merchantId,
            CardHolderName = "John Doe",
            CardNumber = "1234567890234567",
            CardCvv = "345",
            ExpiryYear = "24",
            ExpiryMonth = "04",
            Currency = "USD",
            Amount = 3000
        };

        _encryption.Setup(x => x.Mask(It.IsAny<string>())).Returns("123456******1234");
        _paymentRepository.Setup(x => x.Create(It.IsAny<Payment.Domain.Entities.Payment>()));
        _paymentRepository.Setup(x => x.Update(It.IsAny<Payment.Domain.Entities.Payment>()));

        _merchantRepo.Setup(x => x.GetMerchantById(It.IsAny<Guid>())).Returns(Task.FromResult(new Merchant
        {
            MerchantId = merchantId,
            Description = "test test"
        }));

        _bankClient
            .Setup(x => x.ProcessPayment(It.IsAny<BankRequest>()))
            .Returns(Task.FromResult(new BankResponse
            {
                Reference = "T0234567",
                Status = PaymentStatus.Success
            }));

        //Act
        var payService = new PaymentService(_paymentRepository.Object, _merchantRepo.Object, _encryption.Object, _bankClient.Object);
        var response = await payService.RequestPayment(paymentRequest);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(response.Status, PaymentStatus.Success);
    }
}
