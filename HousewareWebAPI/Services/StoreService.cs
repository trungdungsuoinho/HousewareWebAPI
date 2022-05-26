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
    public interface IStoreService
    {
        public Response GetStore(int storeId);
        public Response GetStores();
        public Response AddStore(AddStoreRequest model);
        public Response UpdateStore(UpdateStoreRequest model);
        public Response DeleteStore(int storeId);
    }

    public class StoreService : IStoreService
    {
        private readonly HousewareContext _context;

        public StoreService(HousewareContext context)
        {
            _context = context;
        }

        private Store GetById(int id)
        {
            return _context.Stores.Where(s => s.StoreId == id).FirstOrDefault();
        }

        public Response GetStore(int storeId)
        {
            Response response = new();
            try
            {
                var store = GetById(storeId);
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                response.SetResult(new GetStoreResponse(store));
                response.SetCode(CodeTypes.Success);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response GetStores()
        {
            Response response = new();
            try
            {
                var stores = _context.Stores.ToList();
                List<GetStoresResponse> storesResponse = new();
                if (stores != null && stores.Count > 0)
                {
                    foreach (var store in stores)
                    {
                        storesResponse.Add(new GetStoresResponse(store));
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(storesResponse);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response AddStore(AddStoreRequest model)
        {
            Response response = new();
            try
            {
                Store store = new()
                {
                    Name = model.Name,
                    Province = model.Province,
                    District = model.District,
                    Ward = model.Ward,
                    Detail = model.Detail
                };
                _context.Stores.Add(store);
                _context.SaveChanges();
                response.SetCode(CodeTypes.Success);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response UpdateStore(UpdateStoreRequest model)
        {
            Response response = new();
            try
            {
                var store = GetById(model.StoreId);
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                store.Name = model.Name;
                store.Province = model.Province;
                store.District = model.District;
                store.Ward = model.Ward;
                store.Detail = model.Detail;
                _context.Entry(store).State = EntityState.Modified;
                _context.SaveChanges();
                response.SetCode(CodeTypes.Success);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response DeleteStore(int storeId)
        {
            Response response = new();
            try
            {
                var store = GetById(storeId);
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                _context.Entry(store).State = EntityState.Deleted;
                _context.SaveChanges();
                response.SetCode(CodeTypes.Success);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }
    }
}
