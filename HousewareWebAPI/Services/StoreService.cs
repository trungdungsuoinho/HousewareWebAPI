using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Services
{
    public interface IStoreService
    {
    }

    public class StoreService : IStoreService
    {
        private readonly HousewareContext _context;
        //private readonly AppSettings _appSettings;

        public StoreService(HousewareContext context/*, IOptions<AppSettings> appSettings*/)
        {
            _context = context;
            //_appSettings = appSettings.Value;
        }

        private Store GetById(int id)
        {
            return _context.Stores.Where(s => s.StoreId == id).FirstOrDefault();
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

        public Response DeleteStore(DeteleStoreRequest model)
        {
            Response response = new();
            try
            {
                var store = GetById(model.StoreId);
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    store.Name = model.Name;
                    store.Province = model.Province;
                    store.District = model.District;
                    store.Ward = model.Ward;
                    store.Detail = model.Detail;
                }
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
    }
}
