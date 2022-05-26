﻿using HousewareWebAPI.Helpers.Attribute;
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
        [MyRequired]
        public string Province { get; set; }
        [MyRequired]
        public string District { get; set; }
        [MyRequired]
        public string Ward { get; set; }
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
