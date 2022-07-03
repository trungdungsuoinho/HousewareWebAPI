using HousewareWebAPI.Helpers.Attribute;
using System;
using System.Text.Json.Serialization;

namespace HousewareWebAPI.Data.Entities
{
    public class Cart
    {
        public Guid CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigation
        [JsonIgnore]
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
