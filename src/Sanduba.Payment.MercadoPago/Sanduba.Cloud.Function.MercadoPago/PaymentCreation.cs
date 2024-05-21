using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Application.Payments.RequestModel;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sanduba.Cloud.Function.MercadoPago
{
    public class PaymentCreation(
        ILogger<PaymentCreation> logger,
        IPaymentRepository paymentRepository,
        IPaymentExternalProvider paymentExternalProvider,
        IPaymentNotification paymentNotification)
    {
        private readonly ILogger<PaymentCreation> _logger = logger;
        private readonly PaymentInteractor _paymentInteractor = new(paymentRepository, paymentExternalProvider, paymentNotification);

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        [Function("PaymentCreation")]
        public IActionResult Create([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var reader = new StreamReader(req.Body).ReadToEndAsync();
            reader.Wait();

            var creationRequest = JsonSerializer.Deserialize<CreatePaymentRequestModel>(reader.Result, jsonOptions);

            if (creationRequest == null)
            {
                return new BadRequestObjectResult("Invalid request sent!");
            }


            var response = _paymentInteractor.CreatePayment(creationRequest);

            return new OkObjectResult(response);
        }

        [Function("PaymentQuery")]
        public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation($"Quering by payment id: {req.Query["id"]}");

            Guid paymentId;

            if (Guid.TryParse(req.Query["id"], out paymentId))
            {
                var queryRequest = new QueryPaymentByIdRequestModel(paymentId);
                var response = _paymentInteractor.GetPaymentById(queryRequest);

                if (response != null)
                {
                    return new OkObjectResult(response);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid Id!");
            }
        }
    }
}
