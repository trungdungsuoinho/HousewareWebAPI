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

        public AddressService(HousewareContext context)
        {
            _context = context;
        }

        private Address GetById(Guid addressId)
        {
            return _context.Addresses.Where(a => a.AddressId == addressId).FirstOrDefault();
        }

        public Response GetAddress(Guid addressId)
        {
            Response response = new();
            try
            {
                var address = _context.Addresses.Where(c => c.AddressId == addressId).FirstOrDefault();
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book");
                    return response;
                }

                GetAddressResponse addressResponse = new()
                {
                    AddressId = address.AddressId,
                    CustomerId = address.CustomerId,
                    Name = address.Name,
                    Company = address.Company,
                    Phone = address.Phone,
                    Province = address.Province,
                    District = address.District,
                    Ward = address.Ward,
                    Detail = address.Detail,
                    Note = address.Note,
                    Type = address.Type,
                    Default = address.Sort == 0
                };

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
                var addresses = _context.Addresses.Where(c => c.CustomerId == customerId).OrderBy(c => c.Sort).ToList();
                GetAddressesResponse addressesResponse = new()
                {
                    CustomerId = customerId,
                    Addresses = new List<AddressResponse>()
                };
                foreach (var address in addresses)
                {
                    addressesResponse.Addresses.Add(new AddressResponse{
                        AddressId = address.AddressId,
                        Name = address.Name,
                        Company = address.Company,
                        Phone = address.Phone,
                        Province = address.Province,
                        District = address.District,
                        Ward = address.Ward,
                        Detail = address.Detail,
                        Note = address.Note,
                        Type = address.Type,
                        Default = address.Sort == 0
                    });
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
            Response response = new();
            try
            {
                _context.Addresses.Add(new Address
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
                    Type = model.Type,
                    Sort = model.Default == true ? 0 : null
                });
                _context.SaveChanges();
                return GetAddresses(model.CustomerId);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateAddress(UpdateAddressRequest model)
        {
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
                address.Sort = model.Default == true ? 0 : null;

                _context.Entry(address).State = EntityState.Modified;
                _context.SaveChanges();
                return GetAddresses(model.CustomerId);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response DeleteAddress(Guid addressId)
        {
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
                _context.Addresses.Remove(address);
                _context.SaveChanges();
                return GetAddresses(address.CustomerId);
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
