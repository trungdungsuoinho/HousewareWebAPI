using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class AddressResponse
    {
        public Guid AddressId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Detail { get; set; }
        public string Note { get; set; }
        public bool Type { get; set; }
        public bool Default { get; set; }
    }

    public class GetAddressesResponse
    {
        public Guid CustomerId { get; set; }
        public List<AddressResponse> Addresses { get; set; }
    }

    public class GetAddressResponse : AddressResponse
    {
        public Guid CustomerId { get; set; }
    }
}
