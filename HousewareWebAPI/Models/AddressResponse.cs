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
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
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
            ProvinceId = address.ProvinceId;
            ProvinceName = address.ProvinceName;
            DistrictId = address.DistrictId;
            DistrictName = address.DistrictName;
            WardId = address.WardId;
            WardName = address.WardName;
            Detail = address.Detail;
            Note = address.Note;
            Type = address.Type;
            Default = address.DefaultCustomer != null;
        }
    }
}
