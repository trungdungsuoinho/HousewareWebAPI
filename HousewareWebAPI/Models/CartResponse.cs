using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class CartResponse
    {
        public Guid CustomerId { get; set; }
        public int TotalPrice { get; set; }
        public List<ProductInCartResponse> Product { get; set; }       
    }

    public class ProductInCartResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ItemPrice { get; set; }
    }
}
