using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class GetCartsRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
    }

    public class AddCartRequest : DeleteCartRequest
    {
        [MyRequired]
        [MyRange(0, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class DeleteCartRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public string ProductId { get; set; }
    }
}
