﻿using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Http;
using Sanduba.Core.Application.Payments.ResponseModel;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Core.Domain.Payments;
using Sanduba.Test.Unit.Commons;
using Sanduba.Test.Unit.Core.Orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Test.Unit.Cloud.Function
{
    public class HttpRequestTest : HttpRequest
    {
        public override HttpContext HttpContext => throw new NotImplementedException();

        public override string Method { get; set; }
        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string? ContentType { get; set; }
        public override Stream Body { get; set; }

        public override bool HasFormContentType => throw new NotImplementedException();

        public override IFormCollection Form { get; set; }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    internal static class PaymentCreationFixture
    {
        internal static HttpRequestTest ValidPaymentCreationHttpRequest() => new HttpRequestTest()
        {
            Method = "POST",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentCreation",
            ContentType = "application/json",
            Body = new MemoryStream(Encoding.UTF8.GetBytes(FileResourceHelper.GetResource(@"Sanduba.Test.Unit.Cloud.Function.Requests.ValidPaymentCreationRequest.json")))
        };

        internal static HttpRequestTest InvalidPaymentCreationHttpRequest() => new HttpRequestTest()
        {
            Method = "POST",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentCreation",
            ContentType = "application/json",
            Body = new MemoryStream(Encoding.UTF8.GetBytes(FileResourceHelper.GetResource(@"Sanduba.Test.Unit.Cloud.Function.Requests.InvalidPaymentCreationRequest.json")))
        };

        internal static HttpRequestTest EmptyHttpRequest() => new HttpRequestTest()
        {
            Method = "POST",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentCreation",
            ContentType = "application/json"
        };

        internal static HttpRequestTest BadFormatOrderHttpRequest() => new HttpRequestTest()
        {
            Method = "POST",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentCreation",
            ContentType = "application/json",
            Body = new MemoryStream(Encoding.UTF8.GetBytes(""))
        };

        internal static HttpRequestTest ValidPaymentQueryHttpRequest() => new HttpRequestTest()
        {
            Method = "GET",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = $"/PaymentQuery",
            Query = new QueryCollection(
                new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()
                {
                    { "id", new Microsoft.Extensions.Primitives.StringValues(new[] { Guid.NewGuid().ToString() }) }
                }),
            ContentType = "application/json"
        };

        internal static HttpRequestTest InvalidPaymentQueryHttpRequest() => new HttpRequestTest()
        {
            Method = "GET",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentQuery",
            Query = new QueryCollection(
                new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()
                {
                    { "id", new Microsoft.Extensions.Primitives.StringValues(new[] { "invalid id" }) }
                }),
            ContentType = "application/json"
        };

        internal static HttpRequestTest EmptyPaymentQueryHttpRequest() => new HttpRequestTest()
        {
            Method = "GET",
            Scheme = "https",
            IsHttps = true,
            Host = new HostString("localhost"),
            PathBase = "/",
            Path = "/PaymentQuery",
            ContentType = "application/json"
        };

        internal static QrCodePaymentData ValidPaymentCreationResponse() => new QrCodePaymentData("ExternalId", "QrCodeData");

        internal static Payment ValidPaymentQueryResponse()
        {
            return new Payment(Guid.NewGuid())
            {
                Order = OrderFixture.CompleteOrder(Guid.NewGuid()),
                Method = Method.PIX,
                Provider = Provider.MercadoPago,
            };
        }
    }
}
