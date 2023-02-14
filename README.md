# Payment Gateway

Payment Gateway API:

## Tools Used:
IDE: Visual studio for mac 2022.
.net core Version: Latest .net 7.0
Database: postgres sql.

## Architecture: 
1. Onion Architecture which is also called Hexagonal architecture.
2. The Solution file contains 6 projects. Paymentent.Api (presentation layer), Payment.Application (Business domain), Payment.Persistence, Payment.Domain, Payment.Banks, and Unit test project
    Payment.Test.
    
# How to run solution.:

1. Open with Visual studio 2022 for window or mac.
2. Build the solution.
3. Set payment.Api as the start up project in the solution.
4. Try to setup Postgre sql if you do not have already using the docker-compose file in the solution using docker command:
   i. docker-compose up.
   ii. I used DBeaver as work bench to open Postgre when it started successfully. 
 
5. Launch the solution i.e Payment.Api using run in visual studio or from command line.
6. The Swagger url will be open automatially to see the REST endpoint available.

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

Takes the reference number from any of the above request and perform Get request.

#Any assumptions you made:

Since we do not have real URL to simulate actual Acquiring bank. I Addedn MockBankSimulator class that implement the interface IBankClient. This help processs the payment. I only add simmulation for Insufficient fund message and others successs. The IBankClient will be implement for different Acquiring bank we have integrated with.

#Areas for improvement:
 1. I will need to add Jwt token authentication or even add two factor authentication.
 2. Can also add some more test like controller test to have full code converage.
 3. I could have also write payment switch simulator using Iso8583 messaging.
 
 # What cloud technologies you’d use and why:
   I will choose Azure cloud technologies due to the following reasons:
    1. Microsoft have great support team based on my previous experience with Azure cloud. Also, my love for Azure cloud.
    2. It fits well into my development workflow, since I use C# and also would have used Azure DEVOps for my CI/CD pipeline. Then easy deployment and          full support from Microsoft team if needed.
    3. The auto scale capability of Azure app service will also be of great benefits. Knowing fully well during the day the API will have more requests
    and I will not have to worry much since Azure can hande for me scale up/down.
    4. Azure function which is the serverless capability provided by azure will also really help for item 3 above. Converting my Payment.Api service to          serverless is pretty easy just by changing the Payment.Api into azure function. Here, I will have more advantage to save cost during the period
       we do not have load request to the API.
 
 

