using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HousewareWebAPI.Services
{
    public interface ICustomerService
    {
        public Response Register(LoginRequest model);
        public LoginCustomerResponse GetLoginRespone(Customer customer);
        public Response Login(LoginRequest model);
        public bool UpdateDefaultAddress(DefaultAddressRequest model);
    }

    public class CustomerService : ICustomerService
    {
        private readonly HousewareContext _context;
        private readonly AppSettings _appSettings;

        public CustomerService(HousewareContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        private Customer GetCusByPhone(string phone)
        {
            return _context.Customers.Where(c => c.Phone == phone).FirstOrDefault();
        }

        public Response Register(LoginRequest model)
        {
            Response response = new();
            try
            {
                var customerExist = GetCusByPhone(model.Phone);
                if (customerExist != null)
                {
                    if (customerExist.VerifyPhone == "Y")
                    {
                        response.SetCode(CodeTypes.Err_Exist);
                        response.SetResult(string.Format(@"Already have an account with this phone number [{0}]!", model.Phone));
                    }
                    else
                    {
                        response.SetCode(CodeTypes.Err_Exist);
                        response.SetResult(string.Format(@"Already have an account with this phone number [{0}] but not yet verified!", model.Phone));
                    }
                    return response;
                }
                Customer customer = new()
                {
                    Phone = model.Phone,
                    Password = model.Password,
                    VerifyPhone = "Y"
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();

                response.SetCode(CodeTypes.Success);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        private string GenerateJwtToken(Customer model)
        {
            if (_appSettings.UsingJWT)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Role", GlobalVariable.RoleCustomer),
                        new Claim("Id", model.CustomerId.ToString())
                    }),
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            return null;
        }

        public LoginCustomerResponse GetLoginRespone(Customer customer)
        {
            LoginCustomerResponse customerResponse = new()
            {
                CustomerId = customer.CustomerId,
                Phone = customer.Phone,
                VerifyPhone = customer.VerifyPhone == "Y",
                Email = customer.Email,
                VerifyEmail = customer.VerifyEmail == "Y",
                FullName = customer.FullName,
                Picture = customer.Picture,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                Token = GenerateJwtToken(customer)
            };
            return customerResponse;
        }

        public Response Login(LoginRequest model)
        {
            Response response = new();
            try
            {
                var customerExist = GetCusByPhone(model.Phone);
                if (customerExist == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult(string.Format(@"No account exists with this phone number [{0}]!", model.Phone));
                    return response;
                }

                if (customerExist.VerifyPhone != "Y")
                {
                    response.SetCode(CodeTypes.Err_Exist);
                    response.SetResult(string.Format(@"Account with this phone number [{0}] has not been verified!", model.Phone));
                }

                response.SetCode(CodeTypes.Success);
                response.SetResult(GetLoginRespone(customerExist));
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public bool UpdateDefaultAddress(DefaultAddressRequest model)
        {
            var customer = _context.Customers.Where(c => c.CustomerId == model.CustomerId).FirstOrDefault();
            if (customer == null)
            {
                throw new Exception("Customer does not exist!");
            }
            customer.DefaultAddressId = model.AddressId;
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
    }
}
