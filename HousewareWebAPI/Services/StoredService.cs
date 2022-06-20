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
        public Response ImportStored(StoredRequest model);
        public Response ExportStored(StoredRequest model);
    }

    public class StoredService : IStoredService
    {
        private readonly HousewareContext _context;

        public StoredService(HousewareContext context)
        {
            _context = context;
        }

        private Store LoadStoreds(StoredRequest stored)
        {
            var store = _context.Stores.Where(s => s.StoreId == stored.StoreId).FirstOrDefault();
            if (store != null)
            {
                if (stored.Products == null && stored.Products.Count > 0)
                {
                    foreach (var product in stored.Products)
                    {
                        _context.Entry(store).Reference(s => s.Storeds.Where(sd => sd.ProductId == product.ProductId)).Load();
                    }
                }
            }

            return store;
        }

        private Stored GetStoredById(int storeId, string productId)
        {
            return _context.Storeds.Where(c => c.StoreId == storeId && c.ProductId == productId).FirstOrDefault();
        }

        private GetStoredResponse GetStoredToResponse(StoredRequest stored)
        {
            _context.Stores.Where(s => s.StoreId == stored.StoreId).FirstOrDefault();
            _context.Entry(stored).Reference(s => s.Product).Load();
            GetStoredResponse storedResponse = new(stored.Store);
            return storedResponse;
        }

        public Response ImportStored(StoredRequest model)
        {
            using var transaction = _context.Database.BeginTransaction();
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

                if (model.Products == null && model.Products.Count > 0)
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
                    }
                    _context.SaveChanges();
                }

                var storedResponse = new ImportStoredResponse(LoadStoreds(model))
                {
                    ImportQuantity = model.Quantity
                };
                transaction.Commit();
                response.SetCode(CodeTypes.Success);
                response.SetResult(storedResponse);
                return response;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response ExportStored(StoredRequest model)
        {
            using var transaction = _context.Database.BeginTransaction();
            Response response = new();
            try
            {
                var stored = GetStoredById(model.StoreId, model.ProductId);
                if (stored == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This product does not stored in the store");
                    return response;
                }

                if (model.Quantity > stored.Quantity)
                {
                    response.SetCode(CodeTypes.Err_IncorrectVal);
                    response.SetResult("The quantity of products exported shipped exceeds the quantity of products stored in the store");
                    return response;
                }

                stored.Quantity -= model.Quantity;
                var storedResponse = new ExportStoredResponse(GetStoredToResponse(stored))
                {
                    ExportQuantity = model.Quantity
                };
                if (stored.Quantity != 0)
                {
                    _context.Entry(stored).State = EntityState.Modified;
                }
                else
                {
                    _context.Entry(stored).State = EntityState.Deleted;
                }
                _context.SaveChanges();
                transaction.Commit();
                response.SetCode(CodeTypes.Success);
                response.SetResult(storedResponse);
                return response;
            }
            catch (Exception e)
            {
                transaction.Rollback();
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
