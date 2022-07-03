using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentType { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
        public string TransactionNo { get; set; }

        // Navigation
        public Guid? CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
        public Guid? AddressId { get; set; }
        public Address Address { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
