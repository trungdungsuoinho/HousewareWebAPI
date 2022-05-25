using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class CartResponse
    {
        public Guid CustomerId { get; set; }
        public uint TotalPrice { get; set; }
        public List<ProInCartResponse> Product { get; set; }       
    }

    public class ProInCartResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public uint Price { get; set; }
        public uint Quantity { get; set; }
        public uint ItemPrice { get; set; }
    }
}
