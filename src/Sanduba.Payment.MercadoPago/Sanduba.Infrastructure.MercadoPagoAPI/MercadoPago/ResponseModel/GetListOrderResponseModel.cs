using System;

namespace Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.ResponseModel
{
    public class GetListOrderResponseModel
    {
        public GetOrderResponseModel[] Elements { get; set; }
        public long NextOffset { get; set; }
        public long Total { get; set; }
    }
}