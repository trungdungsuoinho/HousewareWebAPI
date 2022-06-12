using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class CreateOrderOffineRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public Guid AddressId { get; set; }
        [MyRequired]
        public int StoreId { get; set; }
        public string Note { get; set; }
    }

    public class CreateOrderOnlineRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        [MyRequired]
        public Guid AddressId { get; set; }
        [MyRequired]
        public int StoreId { get; set; }
        public string Note { get; set; }
        [MyRequired]
        public string ReturnUrl { get; set; }
    }
}
