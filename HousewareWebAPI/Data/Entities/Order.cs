using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Data.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string LabelId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string PaymentType { get; set; }
        public bool StatusPaid { get; set; }

        // Navigation
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
