using System;

namespace Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.ResponseModel
{
    public class GetOrderResponseModel
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public Guid ExternalReference { get; set; }
        public string PreferenceId { get; set; }
        public Payment[] Payments { get; set; }
        public object[] Shipments { get; set; }
        public object[] Payouts { get; set; }
        public Collector Collector { get; set; }
        public string Marketplace { get; set; }
        public Uri NotificationUrl { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public object SponsorId { get; set; }
        public long ShippingCost { get; set; }
        public double TotalAmount { get; set; }
        public string SiteId { get; set; }
        public double PaidAmount { get; set; }
        public long RefundedAmount { get; set; }
        public Payer Payer { get; set; }
        public Item[] Items { get; set; }
        public bool Cancelled { get; set; }
        public string AdditionalInfo { get; set; }
        public object ApplicationId { get; set; }
        public bool IsTest { get; set; }
        public string OrderStatus { get; set; }
        public string ClientId { get; set; }
    }

    public class Collector
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CurrencyId { get; set; }
        public string Description { get; set; }
        public object PictureUrl { get; set; }
        public string Title { get; set; }
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
    }

    public class Payer
    {
        public long Id { get; set; }
        public string Email { get; set; }
    }

    public class Payment
    {
        public long Id { get; set; }
        public double TransactionAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public long ShippingCost { get; set; }
        public string CurrencyId { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string OperationType { get; set; }
        public DateTimeOffset DateApproved { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public long AmountRefunded { get; set; }
    }
}
