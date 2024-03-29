﻿using Houseware.WebAPI.Data;
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
        public Response GetSimpleProduct(string id);
        public Response GetProAdmin(string id, bool? enable = null);
        public Response GetAllProAdmin(bool? enable = null);
        public Response GetProAdminByCatId(string id, bool? enable = null);
        public Response AddProAdmin(AddProductRequest model);
        public Response UpdateProAdmin(string id, AddProductRequest model);
        public Response DeleteProAdmin(string id);
        public Response ModifySort(ModifySortProductRequest model);
        public Response Search(SearchProductRequest content, bool toLower = true);
    }
    public class ProductService : IProductService
    {
        private readonly HousewareContext _context;
        private readonly IImageService _imageService;
        private readonly ICategoryService _categoryService;
        private readonly ISpecificationService _specificationService;

        public ProductService(HousewareContext context, IImageService imageService, ICategoryService categoryService, ISpecificationService specificationService)
        {
            _context = context;
            _imageService = imageService;
            _categoryService = categoryService;
            _specificationService = specificationService;
        }

        public Product GetById(string id)
        {
            return _context.Products.FirstOrDefault(c => c.ProductId == id.ToUpper());
        }

        public Response GetSimpleProduct(string id)
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
                    var result = new ProductResponse()
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        Weight = product.Weight,
                        Length = product.Length,
                        Width = product.Width,
                        Height = product.Height,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        Specifications = _specificationService.GetSpecByProduct(product.ProductId)
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
                    var result = new ProductAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        Weight = product.Weight,
                        Length = product.Length,
                        Width = product.Width,
                        Height = product.Height,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        Specifications = _specificationService.GetSpecByProduct(product.ProductId),
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
                var result = new List<ProductAdminResponse>();
                foreach (var product in products)
                {
                    result.Add(new ProductAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        Weight = product.Weight,
                        Length = product.Length,
                        Width = product.Width,
                        Height = product.Height,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        Specifications = _specificationService.GetSpecByProduct(product.ProductId),
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
                var result = new List<ProductAdminResponse>();
                foreach (var product in products)
                {
                    result.Add(new ProductAdminResponse()
                    {
                        ProductId = product.ProductId,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Avatar = product.Avatar,
                        Images = JsonConvert.DeserializeObject<List<string>>(product.Images),
                        Price = product.Price,
                        Weight = product.Weight,
                        Length = product.Length,
                        Width = product.Width,
                        Height = product.Height,
                        View = product.View,
                        Highlights = JsonConvert.DeserializeObject<List<string>>(product.Highlights),
                        Specifications = _specificationService.GetSpecByProduct(product.ProductId),
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

        public Response AddProAdmin(AddProductRequest model)
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
                            Weight = model.Weight,
                            Length = model.Length,
                            Width = model.Width,
                            Height = model.Height,
                            Highlights = JsonConvert.SerializeObject(model.Highlights),
                            Enable = model.Enable == true,
                        };
                        _context.Products.Add(product);
                        _context.SaveChanges();
                        try
                        {
                            if (_specificationService.AddValueSpecification(model.ProductId, model.Specifications))
                            {
                                response.SetCode(CodeTypes.Success);
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception e)
                        {
                            _context.Products.Remove(product);
                            _context.SaveChanges();
                            response.SetCode(CodeTypes.Err_AccFail);
                            response.SetResult("Can't add one of these Specifications. " + e.Message);
                        }
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

        public Response UpdateProAdmin(string id, AddProductRequest model)
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
                        product.Weight = model.Weight;
                        product.Length = model.Length;
                        product.Width = model.Width;
                        product.Height = model.Height;
                        product.Highlights = JsonConvert.SerializeObject(model.Highlights);
                        product.Enable = model.Enable == true;

                        if (_specificationService.UpdateValueSpecification(model.ProductId, model.Specifications))
                        {
                            _context.Entry(product).State = EntityState.Modified;
                            _context.SaveChanges();
                            response.SetCode(CodeTypes.Success);
                        }
                        else
                        {
                            response.SetCode(CodeTypes.Err_AccFail);
                            response.SetResult("Can't update one of these Specifications");
                        }
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

        public Response ModifySort(ModifySortProductRequest model)
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

        public Response Search(SearchProductRequest model, bool toLower = true)
        {
            Response response = new ();
            List<ProductSearchResponse> productsResult = new();

            var products = _context.Products
                .Where(p => p.ProductId.Contains(model.Content) || p.Name.Contains(model.Content))
                .Select(p => new ProductSearchResponse()
                {
                    Product = new(p),
                    Score = p.ProductId.Contains(model.Content) ? 100 : 89
                })
                .ToList();

            var productIds = _context.ProductSpecifications
                .Include(s => s.Specification)
                .Where(s => s.Value.Contains(model.Content) || s.Specification.Name.Contains(model.Content))
                .GroupBy(s => s.ProductId)
                .Select(s => s.Key)
                .ToList();


            response.SetCode(CodeTypes.Success);
            response.SetResult(products);
            return response;
        }
    }
}
