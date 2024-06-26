﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using System;

namespace Sanduba.Cloud.Function.MercadoPago
{
    public class PaymentProviderUpdate(
        ILogger<PaymentProviderUpdate> logger,
        IPaymentRepository paymentRepository,
        IPaymentExternalProvider paymentExternalProvider,
        IPaymentNotification paymentNotification)
    {
        private readonly ILogger<PaymentProviderUpdate> _logger = logger;
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

        [Function("PaymentProviderUpdate")]
        public IActionResult Update([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation($"Payment update query string: {req.QueryString}");
            var reader = new StreamReader(req.Body).ReadToEndAsync();
            reader.Wait();

            var topic = req.Query["topic"];

            if (topic == "merchant_order" && long.TryParse(req.Query["id"], out long id))
            {
                _paymentInteractor.SyncExternalStatus(new UpdatePaymentRequestModel(id));

                _logger.LogInformation($"Payment not processed payload: {reader.Result}");
            }
            else
            {
                _logger.LogInformation($"Payment not processed payload: {reader.Result}");
            }

            return new OkResult();
        }
    }
}
