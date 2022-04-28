using Houseware.WebAPI.Data;
using Houseware.WebAPI.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface ICategoryService
    {
        public Response GetCategory(string id);
        public Response GetCatAdmin(string id, bool? enable = null);
        public Response GetAllCatAdmin(bool? enable = null);
        public Response GetCatAdminByClassId(string id, bool? enable = null);
        public Response AddCatAdmin(AddCatAdminRequest model);
        public Response UpdateCatAdmin(string id, AddCatAdminRequest model);
        public Response DeleteCatAdmin(string id);
        //public Response ModifySort();
    }

    public class CategoryService : ICategoryService
    {
        private readonly HousewareContext _context;
        private readonly IImageService _imageService;

        private Category GetById(string id)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id.ToUpper());
        }

        public CategoryService(HousewareContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public Response GetCategory(string id)
        {
            var response = new Response();
            try
            {
                id = id.ToUpper();
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id && c.Enable == true);
                if (category == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Category was found for this CategoryId");
                }
                else
                {
                    var result = new GetCategoryResponse()
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Advantage = category.Advantage != null ? JArray.Parse(category.Advantage) : null
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

        public Response GetCatAdmin(string id, bool? enable = null)
        {
            var response = new Response();
            try
            {
                id = id.ToUpper();
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id && (enable == null || c.Enable == enable));
                if (category == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Category was found for this CategoryId");
                }
                else
                {
                    var result = new GetCatAdminResponse()
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Slogan = category.Slogan,
                        Image = category.Image,
                        Advantage = category.Advantage != null ? JArray.Parse(category.Advantage) : null,
                        Enable = category.Enable,
                        ClassificationId = category.ClassificationId
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

        public Response GetAllCatAdmin(bool? enable = null)
        {
            var response = new Response();
            try
            {
                var categories = _context.Categories.Where(c => enable == null || c.Enable == enable).OrderBy(c => c.ClassificationId).ThenBy(c => c.Sort).ToList();
                var result = new List<GetCatAdminResponse>();
                foreach (var category in categories)
                {
                    result.Add(new GetCatAdminResponse()
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Slogan = category.Slogan,
                        Image = category.Image,
                        Advantage = category.Advantage != null ? JArray.Parse(category.Advantage) : null,
                        Enable = category.Enable,
                        ClassificationId = category.ClassificationId
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

        public Response GetCatAdminByClassId(string id, bool? enable = null)
        {
            var response = new Response();
            try
            {
                var categories = _context.Categories.Where(c => c.ClassificationId == id.ToUpper() && (enable == null || c.Enable == enable)).OrderBy(c => c.Sort).ToList();
                var result = new List<GetCatAdminResponse>();
                foreach (var category in categories)
                {
                    result.Add(new GetCatAdminResponse()
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Slogan = category.Slogan,
                        Image = category.Image,
                        Advantage = category.Advantage != null ? JArray.Parse(category.Advantage) : null,
                        Enable = category.Enable,
                        ClassificationId = category.ClassificationId
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

        public Response AddCatAdmin(AddCatAdminRequest model)
        {
            var response = new Response();
            try
            {
                if (GetById(model.CategoryId) == null)
                {
                    var category = new Category()
                    {
                        CategoryId = model.CategoryId,
                        Name = model.Name,
                        Slogan = model.Slogan,
                        Image = _imageService.UploadImage(model.Image),
                        //Vdieo = null;
                        Advantage = model.Advantage?.ToString(),
                        Enable = model.Enable,
                        ClassificationId = model.ClassificationId
                    };

                    _context.Categories.Add(category);
                    _context.SaveChanges();

                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_Exist);
                    response.SetResult("There already exists a Category with such CategoryId");
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

        public Response UpdateCatAdmin(string id, AddCatAdminRequest model)
        {
            var response = new Response();
            try
            {
                model.CategoryId = model.CategoryId.ToUpper();
                if (id.ToUpper() != model.CategoryId)
                {
                    response.SetCode(CodeTypes.Err_IdNotMatch);
                    response.SetResult("CategoryId in URL doesn't match CategoryId in model");
                    return response;
                }

                var category = GetById(model.CategoryId);
                if (category != null)
                {
                    category.Name = model.Name;
                    category.Slogan = model.Slogan;
                    if (!model.Image.IsUrl || category.Image != model.Image.Content)
                    {
                        category.Image = _imageService.UploadImage(model.Image);
                    }
                    category.Advantage = model.Advantage?.ToString();
                    category.Enable = model.Enable;
                    category.ClassificationId = model.ClassificationId;

                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Category with such CategoryId");
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

        public Response DeleteCatAdmin(string id)
        {
            var response = new Response();
            try
            {
                var category = GetById(id);
                if (category != null)
                {
                    _context.Entry(category).State = EntityState.Deleted;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                    return response;
                }
                response.SetCode(CodeTypes.Err_NotExist);
                response.SetResult("There not exists a Category with such CategoryId");
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
