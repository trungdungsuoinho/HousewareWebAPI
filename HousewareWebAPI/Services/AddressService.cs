using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IAddressService
    {
        public Response GetAddress(Guid addressId);
        public Response GetAddresses(Guid customerId);
        public Response AddAddress(AddAddressRequest model);
        public Response UpdateAddress(UpdateAddressRequest model);
        public Response DeleteAddress(Guid addressId);
    }

    public class AddressService : IAddressService
    {
        private readonly HousewareContext _context;
        private readonly ICustomerService _customerService;

        public AddressService(HousewareContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        private Address GetById(Guid addressId)
        {
            return _context.Addresses.Where(a => a.AddressId == addressId).Include(a => a.DefaultCustomer).FirstOrDefault();
        }

        public Response GetAddress(Guid addressId)
        {
            Response response = new();
            try
            {
                var address = GetById(addressId);
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book");
                    return response;
                }
                AddressResponse addressResponse = new(address);
                response.SetCode(CodeTypes.Success);
                response.SetResult(addressResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetAddresses(Guid customerId)
        {
            Response response = new();
            try
            {
                List<AddressResponse> addressesResponse = new();
                var addresses = _context.Addresses
                    .Where(c => c.CustomerId == customerId)
                    .Include(a => a.DefaultCustomer)
                    .OrderByDescending(c => c.DefaultCustomer.DefaultAddressId)
                    .ThenBy(c => c.ModifyDate)
                    .ToList();
                if (addresses != null && addresses.Count > 0)
                {
                    foreach (var address in addresses)
                    {
                        addressesResponse.Add(new AddressResponse(address));
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(addressesResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response AddAddress(AddAddressRequest model)
        {
            using var transaction = _context.Database.BeginTransaction();
            Response response = new();
            try
            {
                Address address = new()
                {
                    CustomerId = model.CustomerId,
                    Name = model.Name,
                    Company = model.Company,
                    Phone = model.Phone,
                    Province = model.Province,
                    District = model.District,
                    Ward = model.Ward,
                    Detail = model.Detail,
                    Note = model.Note,
                    Type = model.Type
                };
                _context.Addresses.Add(address);
                _context.SaveChanges();
                if (model.Default == true)
                {
                    DefaultAddressRequest defaultAddress = new()
                    {
                        AddressId = address.AddressId,
                        CustomerId = model.CustomerId
                    };
                    _customerService.UpdateDefaultAddress(defaultAddress);
                    _context.SaveChanges();
                }
                transaction.Commit();
                return GetAddresses(model.CustomerId);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateAddress(UpdateAddressRequest model)
        {
            using var transaction = _context.Database.BeginTransaction();
            Response response = new();
            try
            {
                Address address = new();
                address = GetById(model.AddressId);
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book");
                    return response;
                }
                address.Name = model.Name;
                address.Company = model.Company;
                address.Phone = model.Phone;
                address.Province = model.Province;
                address.District = model.District;
                address.Ward = model.Ward;
                address.Detail = model.Detail;
                address.Note = model.Note;
                address.Type = model.Type;
                _context.Entry(address).State = EntityState.Modified;
                _context.SaveChanges();

                if (model.Default == true)
                {
                    DefaultAddressRequest defaultAddress = new()
                    {
                        AddressId = address.AddressId,
                        CustomerId = address.CustomerId
                    };
                    _customerService.UpdateDefaultAddress(defaultAddress);
                    _context.SaveChanges();
                }
                transaction.Commit();

                return GetAddresses(model.CustomerId);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response DeleteAddress(Guid addressId)
        {
            using var transaction = _context.Database.BeginTransaction();
            Response response = new();
            try
            {
                var address = GetById(addressId);
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address does not exist in the customer's address book");
                    return response;
                }
                if (address.DefaultCustomer != null)
                {
                    DefaultAddressRequest defaultAddress = new()
                    {
                        CustomerId = address.DefaultCustomer.CustomerId
                    };
                    _customerService.UpdateDefaultAddress(defaultAddress);
                }
                _context.Addresses.Remove(address);
                _context.SaveChanges();
                transaction.Commit();
                return GetAddresses(address.CustomerId);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }
    }
}
