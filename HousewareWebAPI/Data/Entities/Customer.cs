﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Phone { get; set; }
        public string VerifyPhone { get; set; }
        public string Email { get; set; }
        public string VerifyEmail { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public DateTime? CreateDate { get; set; }

        // Navigation
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Guid? DefaultAddressId { get; set; }
        [JsonIgnore]
        public Address DefaultAddress { get; set; }

    }
}
