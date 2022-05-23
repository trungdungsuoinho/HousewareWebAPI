﻿using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class GetAddressRequest
    {
        [MyRequired]
        public Guid AddressId { get; set; }
    }

    public class GetAddressesRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
    }

    public class AddAddressRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Company { get; set; }
        [MyRequired]
        [MyRegularExpression(@"^([0-9]{10})$")]
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
        public bool? Default { get; set; }
    }

    public class UpdateAddressRequest
    {
        [MyRequired]
        public Guid AddressId { get; set; }
        [MyRequired]
        public Guid CustomerId { get; set; }
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
        public bool? Default { get; set; }
    }
}
