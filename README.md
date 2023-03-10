# Payment Gateway

Payment Gateway API:

## Tools Used:
IDE: Visual studio for mac 2022.
.net core Version: Latest .net 7.0
Database: postgres sql.

## Architecture: 
1. clean Architecture.
2. The Solution file contains 6 projects. Paymentent.Api (presentation layer), Payment.Application (Business domain), Payment.Persistence, Payment.Domain, Payment.Banks, and Unit test project
    Payment.Test.
    
# How to run solution.:

1. Open with Visual studio 2022 for window or mac.
2. Build the solution.
3. Set payment.Api as the start up project in the solution.
4. Try to setup Postgre sql if you do not have already using the docker-compose file in the solution using docker command:
   i. docker-compose up.
   ii. I used DBeaver as work bench to open Postgre when it started successfully. 
5. Open the Payment.Persistence in terminal and run the command dotnet ef database update
6. Launch the solution i.e Payment.Api using run in visual studio or from command line.
7. The Swagger url will be open automatially to see the REST endpoint available.

### The Payment.Api expose two REST endpoint

1. Post request. To submit payment request from merchant to Payment gateway API.
2. Get request. To get payment detail from Paymentgateway API usng the payment reference.

Note: The Payment.Persistence have seed data for merchant inside configuration folder.

### To simulate we can make sample API request to the API.

1. Post Request: /api/Payments

i. Post Request to simulate successfull payment
  {
    "merchantId": "dc187117-7e53-4623-a59e-940430403f64",
    "cardHolderName": "John Doe",
    "cardNumber": "1674818961384534", // not this is 16 digit card number
    "cardCvv": "754",
    "expiryYear": "st",
    "expiryMonth": "st",
    "amount": 500,
    "currency": "str"
  }

ii. Post Request to simulate failed due to insufficient fund in another card number:
{
  "merchantId": "dc187117-7e53-4623-a59e-940430403f64",
  "cardHolderName": "Moses Doe",
  "cardNumber": "5134123412341234", // not this is 16 digit card number
  "cardCvv": "754",
  "expiryYear": "st",
  "expiryMonth": "st",
  "amount": 500,
  "currency": "str"
}

Response containt payment reference number and status.
Response:
{
    "reference": "reference number.",
    "status": 1 
}

For simplicity Status are :
0 => Pending
1 => Success
2 => Failed

2. Get Request: /api/Payments/{reference}
