using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string SortCode { get; set; }
        public StoreResponse Store { get; set; }
        public AddressResponse Address { get; set; }
        public List<ProInCartResponse> Products { get; set; } = new();
        public uint TotalPrice { get; set; }
        public uint TotalFee { get; set; }
        public uint Total { get; set; }
        public DateTime ExpectedDeliveryTime { get; set; }
    }
}
