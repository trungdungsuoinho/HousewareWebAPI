using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IStoredService
    {
        public Response GetStored(StoredRequest model);
        public Response ImportStored(ChangeStoredRequest model);
        public Response ExportStored(ChangeStoredRequest model);
    }

    public class StoredService : IStoredService
    {
        private readonly HousewareContext _context;

        public StoredService(HousewareContext context)
        {
            _context = context;
        }

        private Stored GetStoredById(int storeId, string productId)
        {
            return _context.Storeds.Where(c => c.StoreId == storeId && c.ProductId == productId).Include(s => s.Product).FirstOrDefault();
        }

        public Response GetStored(StoredRequest model)
        {
            Response response = new();
            try
            {
                var store = _context.Stores.Where(s => s.StoreId == model.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                GetStoredResponse getStoredResponse = new(store);

                if (model.Products != null && model.Products.Count > 0)
                {
                    foreach (var product in model.Products)
                    {
                        var stored = GetStoredById(model.StoreId, product.ProductId);
                        if (stored == null)
                        {
                            response.SetCode(CodeTypes.Err_NotExist);
                            response.SetResult("This product does not stored in the store");
                            return response;
                        }
                        getStoredResponse.Products.Add(new ProGetStoredResponse(stored));
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(getStoredResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response ImportStored(ChangeStoredRequest model)
        {
            Response response = new();
            try
            {
                var store = _context.Stores.Where(s => s.StoreId == model.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                GetChangeStoredResponse getStoredResponse = new(store);

                if (model.Products != null && model.Products.Count > 0)
                {
                    foreach (var product in model.Products)
                    {
                        var stored = GetStoredById(model.StoreId, product.ProductId);
                        if (stored != null)
                        {
                            stored.Quantity += product.Quantity;
                            _context.Entry(stored).State = EntityState.Modified;
                        }
                        else
                        {
                            stored = new Stored
                            {
                                StoreId = model.StoreId,
                                ProductId = product.ProductId,
                                Quantity = product.Quantity
                            };
                            _context.Storeds.Add(stored);
                        }
                        getStoredResponse.Products.Add(new ProStoredResponse(stored, product.Quantity));
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        response.SetCode(CodeTypes.Err_AccFail);
                        response.SetResult(e.Message);
                        return response;
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(getStoredResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response ExportStored(ChangeStoredRequest model)
        {
            Response response = new();
            try
            {
                var store = _context.Stores.Where(s => s.StoreId == model.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Store with such StoreId");
                    return response;
                }
                GetChangeStoredResponse getStoredResponse = new(store);

                if (model.Products != null && model.Products.Count > 0)
                {
                    foreach (var product in model.Products)
                    {
                        var stored = GetStoredById(model.StoreId, product.ProductId);
                        if (stored == null)
                        {
                            response.SetCode(CodeTypes.Err_NotExist);
                            response.SetResult("This product does not stored in the store");
                            return response;
                        }

                        if (product.Quantity > stored.Quantity)
                        {
                            response.SetCode(CodeTypes.Err_IncorrectVal);
                            response.SetResult("The quantity of products exported shipped exceeds the quantity of products stored in the store");
                            return response;
                        }

                        stored.Quantity -= product.Quantity;
                        if (stored.Quantity != 0)
                        {
                            _context.Entry(stored).State = EntityState.Modified;
                        }
                        else
                        {
                            _context.Entry(stored).State = EntityState.Deleted;
                        }
                        getStoredResponse.Products.Add(new ProStoredResponse(stored, product.Quantity));
                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        response.SetCode(CodeTypes.Err_AccFail);
                        response.SetResult(e.Message);
                        return response;
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(getStoredResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetAvailableStoreds(GetCartsRequest model)
        {
            Response response = new();
            try
            {
                var carts = _context.Carts.Where(c => c.CustomerId == model.CustomerId).ToList();
                var storeds = from a in carts
                              join s in _context.Storeds.ToList()
                              on a.ProductId equals s.ProductId
                              where a.Quantity <= s.Quantity
                              select s;
                foreach (var stored in storeds)
                {

                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(storeds);
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
