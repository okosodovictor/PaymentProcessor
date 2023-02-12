using Microsoft.AspNetCore.Mvc;
using Payment.Api.Models;
using Payment.Application.Business;
using Payment.Application.Interfaces;
using Payment.Domain.Entities;
using Payment.Domain.Exceptions;

namespace Payment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly IPaymentService _service;

    public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentRequest model)
    {
        try
        {
            var response = await _service.RequestPayment(model);
            return Ok(response);
        }
        catch (ArgumentException agex)
        {
            _logger.LogError(agex.Message);
            return BadRequest(agex);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogError(nfex.Message);
            return NotFound(nfex.Message);
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> Get(string reference)
    {
        if (string.IsNullOrEmpty(reference))
        {
            return BadRequest("Payment Reference is Empty");
        }
        var response = await _service.GetPaymentByReference(reference);
        if (response == null)
        {
            return NotFound($"No Payment exist with Reference Number:  {reference}");
        }
        else
        {
            return Ok(new PaymentDto
            {
                Reference = response.Reference,
                Status = response.Status.ToString(),
                Amount = response.Amount,
                CardNumber = response.CardNumber,
                Currency = response.Currency,
                CardHolderName = response.CardHolderName,
                ExpiringDate = response.ExpiryMonth + "/" + response.ExpiryYear,
                merchant = new MerchantDto
                {
                    MerchantId = response.Merchant.MerchantId,
                    MerchantName = response.Merchant.MerchantName,
                    Description = response.Merchant.Description,
                    AcquirerBank = response.Merchant.AcquirerBank
                }
            });
        }
    }
}

