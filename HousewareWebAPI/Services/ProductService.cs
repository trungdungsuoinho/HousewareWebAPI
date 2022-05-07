using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IProductService
    {
        public Product GetById(string id);
        public Response GetProduct(string id);
        public Response GetProAdmin(string id, bool? enable = null);
        public Response GetAllProAdmin(bool? enable = null);
        public Response GetProAdminByCatId(string id, bool? enable = null);
        public Response AddProAdmin(AddProAdminRequest model);
        public Response UpdateProAdmin(string id, AddProAdminRequest model);
        public Response DeleteProAdmin(string id);
        public Response ModifySort(ModifySortProAdminRequest model);
    }
    public class ProductService : IProductService
    {
        private readonly HousewareContext _context;
        private readonly IImageService _imageService;
        private readonly ICategoryService _categoryService;

        public ProductService(HousewareContext context, IImageService imageService, ICategoryService categoryService)
        {
            _context = context;
            _imageService = imageService;
            _categoryService = categoryService;
        }

        public Product GetById(string id)
        {
            return _context.Products.FirstOrDefault(c => c.ProductId == id.ToUpper());
        }

        public Response GetProduct(string id)
        {
            var response = new Response();
            try
            {
                var product = _context.Products.FirstOrDefault(c => c.ProductId == id.ToUpper() && c.Enable == true);
                if (product == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Category was found for this CategoryId");
                }
                else
                {
                    //_context.Entry(product).Collection(c => c.ProductSpecifications).Load();
                    var result = new GetProductResponse()
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        //ProductSpecifications = product.ProductSpecifications.OrderBy(c => c.Sort).Select(c => new ProInGetCat
                        //{
                        //    ProductId = c.ProductId,
                        //    Name = c.Name,
                        //    Avatar = c.Avatar,
                        //    Price = c.Price
                        //}).ToList()   ChuaLam
                    };
                    response.SetCode(CodeTypes.Success);
                    response.SetResult(result);
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetProAdmin(string id, bool? enable = null)
        {
            var response = new Response();
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id.ToUpper() && (enable == null || p.Enable == enable));
                if (product == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Product was found for this ProductId");
                }
                else
                {
                    var result = new GetProAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        CreateDate = product.CreateDate,
                        ModifyDate = product.ModifyDate,
                        Enable = product.Enable
                    };
                    response.SetCode(CodeTypes.Success);
                    response.SetResult(result);
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetAllProAdmin(bool? enable = null)
        {
            var response = new Response();
            try
            {
                var products = _context.Products.Where(c => enable == null || c.Enable == enable).OrderBy(c => c.ProductId).ThenBy(c => c.Sort).ToList();
                var result = new List<GetProAdminResponse>();
                foreach (var product in products)
                {
                    result.Add(new GetProAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        CreateDate = product.CreateDate,
                        ModifyDate = product.ModifyDate,
                        Enable = product.Enable
                    });
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(result);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response GetProAdminByCatId(string id, bool? enable = null)
        {
            var response = new Response();
            try
            {
                var products = _context.Products.Where(c => c.CategoryId == id.ToUpper() && (enable == null || c.Enable == enable)).OrderBy(c => c.Sort).ToList();
                var result = new List<GetProAdminResponse>();
                foreach (var product in products)
                {
                    result.Add(new GetProAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        CreateDate = product.CreateDate,
                        ModifyDate = product.ModifyDate,
                        Enable = product.Enable
                    });
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(result);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response AddProAdmin(AddProAdminRequest model)
        {
            var response = new Response();
            try
            {
                if (GetById(model.ProductId) == null)
                {
                    if (_categoryService.GetById(model.CategoryId) != null)
                    {
                        var product = new Product()
                        {
                            ProductId = model.ProductId,
                            CategoryId = model.CategoryId,
                            Name = model.Name,
                            Avatar = _imageService.UploadImage(model.Avatar),
                            Images = JsonConvert.SerializeObject(_imageService.UploadImages(model.Images)),
                            Price = model.Price,
                            Highlights = JsonConvert.SerializeObject(model.Highlights),
                            Enable = model.Enable == true,
                        };
                        _context.Products.Add(product);
                        _context.SaveChanges();
                        response.SetCode(CodeTypes.Success);
                    }
                    else
                    {
                        response.SetCode(CodeTypes.Err_NotExist);
                        response.SetResult("This CategoryId doesn't exist");
                    }
                }
                else
                {
                    response.SetCode(CodeTypes.Err_Exist);
                    response.SetResult("There already exists a Product with such ProductId");
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateProAdmin(string id, AddProAdminRequest model)
        {
            var response = new Response();
            try
            {
                if (string.Compare(id, model.ProductId, true) != 0)
                {
                    response.SetCode(CodeTypes.Err_IdNotMatch);
                    response.SetResult("ProductId in URL doesn't match ProductId in model");
                    return response;
                }

                var product = GetById(model.ProductId);
                if (product != null)
                {
                    if (_categoryService.GetById(model.CategoryId) != null)
                    {
                        product.Name = model.Name;
                        product.CategoryId = model.CategoryId;
                        if (!model.Avatar.IsUrl || product.Avatar != model.Avatar.Content)
                        {
                            product.Avatar = _imageService.UploadImage(model.Avatar);
                        }
                        product.Images = JsonConvert.SerializeObject(_imageService.UploadImages(model.Images)).ToString();
                        product.Price = model.Price;
                        product.Highlights = JsonConvert.SerializeObject(model.Highlights);
                        product.Enable = model.Enable == true;
                        
                        _context.Entry(product).State = EntityState.Modified;
                        _context.SaveChanges();
                        response.SetCode(CodeTypes.Success);
                    }
                    else
                    {
                        response.SetCode(CodeTypes.Err_NotExist);
                        response.SetResult("This CategoryId doesn't exist");
                    }
                }
                else
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Product with such ProductId");
                }
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response DeleteProAdmin(string id)
        {
            var response = new Response();
            try
            {
                var product = GetById(id);
                if (product != null)
                {
                    _context.Entry(product).State = EntityState.Deleted;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                    return response;
                }
                response.SetCode(CodeTypes.Err_NotExist);
                response.SetResult("There not exists a Product with such ProductId");
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response ModifySort(ModifySortProAdminRequest model)
        {
            var response = new Response();
            var products = _context.Products.Where(c => c.CategoryId == model.CategoryId.ToUpper()).ToList();
            foreach (var product in products)
            {
                var id = model.ProductIds.Where(i => i.ToUpper() == product.ProductId).FirstOrDefault();
                product.Sort = id != null ? model.ProductIds.IndexOf(id) : int.MaxValue;
                _context.Entry(product).State = EntityState.Modified;
            }
            _context.SaveChanges();
            response.SetCode(CodeTypes.Success);
            return response;
        }
    }
}
