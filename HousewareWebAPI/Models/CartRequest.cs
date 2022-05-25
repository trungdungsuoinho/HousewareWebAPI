using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class GetCartRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
    }

    public class AddProIntoCartRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public string ProductId { get; set; }
        [MyRequired]
        [MyRange(1, int.MaxValue)]
        public uint Quantity { get; set; }
    }

    public class DeleteProInCartRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public string ProductId { get; set; }
    }
}
