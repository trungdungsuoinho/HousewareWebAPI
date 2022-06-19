using HousewareWebAPI.Helpers.Attribute;
using System;

namespace HousewareWebAPI.Models
{
    public class LoginRequest
    {
        [MyRequired]
        [MyRegularExpression(@"^([0-9]{10})$")]
        public string Phone { get; set; }
        [MyRequired]
        public string Password { get; set; }
    }

    public class DefaultAddressRequest
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
        public Guid? AddressId { get; set; }
    }

    public class GetCustomer
    {
        [MyRequired]
        public Guid CustomerId { get; set; }
    }

    public class UpdateCustomer
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public ImageInput Picture { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool? Gender { get; set; }
    }
}
