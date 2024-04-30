using Microsoft.Extensions.Options;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Core.Domain.Payments;
using Sanduba.Infrastructure.MercadoPagoAPI.Configurations.Options;
using Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.RequestModel;
using Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.ResponseModel;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using OrderItem = Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.RequestModel.OrderItem;
using Payment = Sanduba.Core.Domain.Payments.Payment;

namespace Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago
{
    public class MercadoPagoApiGateway(IOptions<MercadoPagoOptions> options) 
        : IPaymentExternalProvider
    {
        MercadoPagoOptions _options = options.Value;
        string _qrRequestUri = "instore/orders/qr/seller/collectors";
        string _paymentDetailRequestUri = "merchant_orders";

        public async Task<QrCodePaymentData> CreateQrCodePayment(Payment payment)
        {
            string requestUrl = $"{_options.BaseUrl}/{_qrRequestUri}/{_options.UserId}/pos/{_options.CashierId}/qrs";
            
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {_options.AutheticationToken}");


            var paymentPayload = JsonSerializer.Serialize(TransalateToExternalRequest(payment), jsonOptions);
            var content = new StringContent(paymentPayload);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var reader = response.Content.ReadAsStringAsync();
            reader.Wait();

            var creationResponse = JsonSerializer.Deserialize<CreatePixPaymentResponseModel>(reader.Result, jsonOptions);

            if(creationResponse != null)
            {
                var externalId = GetExternalId(payment.Id);

                if (externalId != null)
                {
                    var paymentData = new QrCodePaymentData(externalId.Result.ToString(), creationResponse.QrData);
                    return paymentData;
                }   
            }

            return null;
        }

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            WriteIndented = true,
        };

        private CreatePixPaymentRequestModel TransalateToExternalRequest(Payment payment)
        {
            return new CreatePixPaymentRequestModel(
                Description: $"Pedido No {payment.Order.Code} feito {DateTime.Today.ToString("dd/MM/yyyy")}",
                ExternalReference: payment.Id.ToString(),
                Items: payment.Order.Items
                                .Select(i => new OrderItem(                              
                                    Category: i.Product.Category.ToString(),
                                    Title: i.Product.Name,
                                    Description: $"Item {i.Code} - {i.Product.Name}",
                                    UnitPrice:  i.Product.UnitPrice,
                                    Quantity: 1,
                                    UnitMeasure: "unit",
                                    TotalAmount: i.Amount
                                )).ToArray(),
                NotificationUrl:  _options.NotificationUrl,
                Title: $"Restaudante Sanduba - Pedido No {payment.Order.Code}",
                TotalAmount: payment.Order.TotalAmount,
                ExpirationDate: DateTimeOffset.Now.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ss.fffzzzz")
            );
        }

        private async Task<long> GetExternalId(Guid paymentId)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5)); // TODO: atualizar via webhook

            string requestUrl = $"{_options.BaseUrl}/{_paymentDetailRequestUri}?external_reference={paymentId}";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {_options.AutheticationToken}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var reader = response.Content.ReadAsStringAsync();
            reader.Wait();

            var paymentDetailResponse = JsonSerializer.Deserialize<GetListOrderResponseModel>(reader.Result, jsonOptions);

            return paymentDetailResponse.Elements[0].Id;
        }

        public async Task<PaymentDetailData> GetPaymentData(Payment payment)
        {
            string requestUrl = $"{_options.BaseUrl}/{_paymentDetailRequestUri}/{payment.ExternalPaymentProvider.ExternalId}";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {_options.AutheticationToken}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var reader = response.Content.ReadAsStringAsync();
            reader.Wait();

            var paymentDetailResponse = JsonSerializer.Deserialize<GetOrderResponseModel>(reader.Result, jsonOptions);

            if (paymentDetailResponse?.Payments is null 
                || paymentDetailResponse.Payments.Length < 1)
            {
                return new PaymentDetailData(PaymentStatus.WaitingPayment, null, null);
            }

            var paymentId = paymentDetailResponse.Payments[0].Id;
            var payed = paymentDetailResponse.Payments[0].Status == "approved";
            var payedAt = paymentDetailResponse.Payments[0].DateApproved;

            if (payed) return new PaymentDetailData(PaymentStatus.Payed, paymentId.ToString(), payedAt);
            else return new PaymentDetailData(PaymentStatus.Cancelled, null, null);
        }
    }
}
