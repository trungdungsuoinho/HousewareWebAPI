using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
        //public Response UpdateStore(UpdateStoreRequest model);
        //public Response DeleteStore(int storeId);
    }

    public class StoreService : IStoreService
    {
        private readonly HousewareContext _context;
        private readonly IGHNService _gHNService;

        public StoreService(HousewareContext context, IGHNService gHNService)
        {
            _context = context;
            _gHNService = gHNService;
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
                List<GetStoreResponse> storesResponse = new();
                if (stores != null && stores.Count > 0)
                {
                    foreach (var store in stores)
                    {
                        storesResponse.Add(new GetStoreResponse(store));
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
            using var transaction = _context.Database.BeginTransaction();
            Response response = new();
            try
            {
                Store store = new()
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    ProvinceId = model.ProvinceId,
                    ProvinceName = model.ProvinceName,
                    DistrictId = model.DistrictId,
                    DistrictName = model.DistrictName,
                    WardId = model.WardId,
                    WardName = model.WardName,
                    Detail = model.Detail
                };
                _context.Stores.Add(store);
                _context.SaveChanges();

                GHNRegisterShopRequest registerShop = new()
                {
                    District_id = model.DistrictId,
                    Ward_code = model.WardId,
                    Name = model.Name,
                    Phone = model.Phone,
                    Address = model.Detail
                };

                try
                {
                    store.ShopId = _gHNService.RegisterShop(registerShop);
                    _context.Entry(store).State = EntityState.Modified;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    response.SetCode(CodeTypes.Err_AccFail);
                    response.SetResult("Create a store in GiaoHangNhanh service failed! " + e.Message);
                    transaction.Rollback();
                }
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                transaction.Rollback();
            }
            return response;
        }

        //public Response UpdateStore(UpdateStoreRequest model)
        //{
        //    Response response = new();
        //    try
        //    {
        //        var store = GetById(model.StoreId);
        //        if (store == null)
        //        {
        //            response.SetCode(CodeTypes.Err_NotExist);
        //            response.SetResult("There not exists a Store with such StoreId");
        //            return response;
        //        }
        //        store.Name = model.Name;
        //        store.Province = model.Province;
        //        store.District = model.District;
        //        store.Ward = model.Ward;
        //        store.Detail = model.Detail;
        //        _context.Entry(store).State = EntityState.Modified;
        //        _context.SaveChanges();
        //        response.SetCode(CodeTypes.Success);
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //    }
        //    return response;
        //}

        //public Response DeleteStore(int storeId)
        //{
        //    Response response = new();
        //    try
        //    {
        //        var store = GetById(storeId);
        //        if (store == null)
        //        {
        //            response.SetCode(CodeTypes.Err_NotExist);
        //            response.SetResult("There not exists a Store with such StoreId");
        //            return response;
        //        }
        //        _context.Entry(store).State = EntityState.Deleted;
        //        _context.SaveChanges();
        //        response.SetCode(CodeTypes.Success);
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //    }
        //    return response;
        //}
    }
}
