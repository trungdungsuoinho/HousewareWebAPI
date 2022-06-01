using HousewareWebAPI.Helpers.Attribute;
using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Company { get; set; }
        [MyRequired]
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        [MyRequired]
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        [MyRequired]
        public string DistrictName { get; set; }
        [MyRequired]
        public string WardId { get; set; }
        [MyRequired]
        public string WardName { get; set; }
        [MyRequired]
        public string Detail { get; set; }
        public string Note { get; set; }
        public bool Type { get; set; }
        public DateTime ModifyDate { get; set; }

        // Navigation
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Customer DefaultCustomer { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
