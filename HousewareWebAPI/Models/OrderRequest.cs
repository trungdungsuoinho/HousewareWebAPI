using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class CreateOrderRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public Guid AddressId { get; set; }
        [MyRequired]
        public string Note { get; set; }
    }

    public class CreateOrderOnlineRequest: CreateOrderRequest
    {
        [MyRequired]
        public string ReturnUrl { get; set; }
    }

    public class OrderIdRequest
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrdersRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        public string Status { get; set; }
        [MyRange(0, int.MaxValue)]
        public int Page { get; set; } = 0;
        [MyRange(1, int.MaxValue)]
        public int Size { get; set; } = 10;
    }
}
