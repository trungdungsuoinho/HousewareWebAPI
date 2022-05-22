using System;

namespace HousewareWebAPI.Models
{
    public class LoginCustomerResponse
    {
        public Guid CustomerId { get; set; }
        public string Phone { get; set; }
        public bool VerifyPhone { get; set; }
        public string Email { get; set; }
        public bool VerifyEmail { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string Token { get; set; }
    }
}
