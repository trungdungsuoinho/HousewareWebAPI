using Houseware.WebAPI.Data;
using Houseware.WebAPI.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using System;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface ICategoryService
    {
        public Response AddCategory(AddCategoryRequest model);
        //public Response GetClassification(string id, bool? enable = null);
        //public Response GetAllClassification(bool? enable = null);
        //public Response UpdateClassification(string id, AddClassificationRequest model);
        //public Response DeleteClassification(string id);
        ////public Response ModifySort();
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

        public Response AddCategory(AddCategoryRequest model)
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
                        Advantage = model.Advantage?.ToString(),
                        Enable = model.Enable
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

        //public Response GetClassification(string id, bool? enable = null)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        id = id.ToUpper();
        //        var classification = _context.Classifications.FirstOrDefault(c => c.ClassificationId == id && (enable == null || c.Enable == enable));
        //        if (classification == null)
        //        {
        //            response.SetCode(CodeTypes.Err_NotFound);
        //            response.SetResult("No Classification was found for this ClassificationId");
        //        }
        //        else
        //        {
        //            var result = new GetClassificationResponse()
        //            {
        //                ClassificationId = classification.ClassificationId,
        //                Name = classification.Name,
        //                ImageMenu = classification.ImageMenu,
        //                ImageBanner = classification.ImageBanner
        //            };
        //            response.SetCode(CodeTypes.Success);
        //            response.SetResult(result);
        //        }
        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //        return response;
        //    }
        //}

        //public Response GetAllClassification(bool? enable = null)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        var classifications = _context.Classifications.Where(c => enable == null || c.Enable == enable).OrderBy(c => c.Sort).ToList();
        //        var result = new List<GetClassificationResponse>();
        //        foreach (var c in classifications)
        //        {
        //            result.Add(new GetClassificationResponse()
        //            {
        //                ClassificationId = c.ClassificationId,
        //                Name = c.Name,
        //                ImageMenu = c.ImageMenu,
        //                ImageBanner = c.ImageBanner
        //            });
        //        }
        //        response.SetCode(CodeTypes.Success);
        //        response.SetResult(result);
        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //        return response;
        //    }
        //}

        //public Response UpdateClassification(string id, AddClassificationRequest model)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        model.ClassificationId = model.ClassificationId.ToUpper();
        //        if (id.ToUpper() != model.ClassificationId)
        //        {
        //            response.SetCode(CodeTypes.Err_IdNotMatch);
        //            response.SetResult("ClassificationId in URL doesn't match ClassificationId in model");
        //            return response;
        //        }

        //        var classification = GetById(model.ClassificationId);
        //        if (classification != null)
        //        {
        //            classification.Name = model.Name;

        //            if (!model.ImageMenu.IsUrl || classification.ImageMenu != model.ImageMenu.Content)
        //            {
        //                classification.ImageMenu = _imageService.UploadImage(model.ImageMenu);
        //            }

        //            if (model.ImageBanner != null)
        //            {
        //                if (!model.ImageBanner.IsUrl || classification.ImageBanner != model.ImageBanner.Content)
        //                {
        //                    classification.ImageBanner = _imageService.UploadImage(model.ImageBanner);
        //                }
        //            }
        //            else
        //            {
        //                classification.ImageBanner = null;
        //            }
        //            classification.Story = model.Story?.ToString();
        //            classification.Enable = model.Enable;

        //            _context.Entry(classification).State = EntityState.Modified;
        //            _context.SaveChanges();
        //            response.SetCode(CodeTypes.Success);
        //        }
        //        else
        //        {
        //            response.SetCode(CodeTypes.Err_NotExist);
        //            response.SetResult("There not exists a Classification with such ClassificationId");
        //        }
        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //        return response;
        //    }
        //}

        //public Response DeleteClassification(string id)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        var classification = GetById(id);
        //        if (classification != null)
        //        {
        //            _context.Entry(classification).State = EntityState.Deleted;
        //            _context.SaveChanges();
        //            response.SetCode(CodeTypes.Success);
        //            return response;
        //        }
        //        response.SetCode(CodeTypes.Err_NotExist);
        //        response.SetResult("There not exists a Classification with such ClassificationId");
        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        response.SetCode(CodeTypes.Err_Exception);
        //        response.SetResult(e.Message);
        //        return response;
        //    }
        //}
    }
}
