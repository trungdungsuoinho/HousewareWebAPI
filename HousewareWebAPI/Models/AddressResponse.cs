using HousewareWebAPI.Data.Entities;
using System;

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
        public AddressResponse(Address address)
        {
            AddressId = address.AddressId;
            Name = address.Name;
            Company = address.Company;
            Phone = address.Phone;
            Province = address.Province;
            District = address.District;
            Ward = address.Ward;
            Detail = address.Detail;
            Note = address.Note;
            Type = address.Type;
            Default = address.DefaultCustomer != null;
        }
    }
}
