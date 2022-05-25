using HousewareWebAPI.Helpers.Attribute;
using System;
using System.Text.Json.Serialization;

namespace HousewareWebAPI.Data.Entities
{
    public class Cart
    {
        public Guid CustomerId { get; set; }
        public string ProductId { get; set; }
        [MyRange(0, int.MaxValue)]
        public uint Quantity { get; set; }

        // Navigation
        [JsonIgnore]
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
