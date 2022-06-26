using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
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
        public LoginCustomerResponse GetLoginResponse(Customer customer);
        public Response Login(LoginRequest model);
        public bool UpdateDefaultAddress(DefaultAddressRequest model);
        public Response GetCustomerInfo(GetCustomer model);
        public Response UpdateCustomerInfo(UpdateCustomer model);
    }

    public class CustomerService : ICustomerService
    {
        private readonly HousewareContext _context;
        private readonly AppSettings _appSettings;
        private readonly IImageService _imageService;

        public CustomerService(HousewareContext context, IOptions<AppSettings> appSettings, IImageService imageService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _imageService = imageService;
        }

        private Customer GetCusByPhone(string phone)
        {
            return _context.Customers.Where(c => c.Phone == phone).FirstOrDefault();
        }

        private Customer GetCusById(Guid id)
        {
            return _context.Customers.Where(c => c.CustomerId == id).FirstOrDefault();
        }

        public Response Register(LoginRequest model)
        {
            Response response = new();
            try
            {
                var customer = GetCusByPhone(model.Phone);
                if (customer != null)
                {
                    if (customer.VerifyPhone == "Y")
                    {
                        response.SetCode(CodeTypes.Err_Exist);
                        response.SetResult(string.Format(@"Already have an account with this phone number [{0}]!", model.Phone));
                        return response;
                    }
                    else
                    {
                        //Create verify
                        response.SetCode(CodeTypes.Err_Exist);
                        response.SetResult(string.Format(@"Already have an account with this phone number [{0}] but not yet verified!", model.Phone));
                    }
                }
                else
                {
                    customer = new()
                    {
                        FullName = model.Phone,
                        Phone = model.Phone,
                        Password = model.Password,
                    };
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }


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

        public LoginCustomerResponse GetLoginResponse(Customer customer)
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
                    return response;
                }

                response.SetCode(CodeTypes.Success);
                response.SetResult(GetLoginResponse(customerExist));
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

        public GetCustomerResponse GetCustomerResponse(Customer customer)
        {
            GetCustomerResponse customerResponse = new()
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
            };
            return customerResponse;
        }

        public Response GetCustomerInfo(GetCustomer model)
        {
            Response response = new();
            try
            {
                var customer = GetCusById(model.CustomerId);
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult(string.Format(@"No account exists with this id [{0}]!", model.CustomerId));
                    return response;
                }

                response.SetCode(CodeTypes.Success);
                response.SetResult(GetCustomerResponse(customer));
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateCustomerInfo(UpdateCustomer model)
        {
            Response response = new();
            try
            {
                var customer = GetCusById(model.CustomerId);
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult(string.Format(@"No account exists with this id [{0}]!", model.CustomerId));
                    return response;
                }

                customer.FullName = model.FullName;
                customer.Picture = _imageService.UploadImage(model.Picture);
                customer.DateOfBirth = model.DateOfBirth;
                customer.Gender = model.Gender;
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();

                response.SetCode(CodeTypes.Success);
                response.SetResult(GetCustomerResponse(customer));
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }
    }
}
