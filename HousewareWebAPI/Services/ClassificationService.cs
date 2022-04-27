using Houseware.WebAPI.Data;
using Houseware.WebAPI.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IClassificationService
    {
        public Response AddClassification(AddClassificationRequest model);
        public Response GetClassification(string id, bool? enable = null);
        public Response GetAllClassification(bool? enable = null);
        public Response UpdateClassification(string id, AddClassificationRequest model);
        public Response DeleteClassification(string id);
        //public Response ModifySort();
    }
    public class ClassificationService : IClassificationService
    {
        private readonly HousewareContext _context;
        private readonly IImageService _imageService;

        private Classification GetById(string id)
        {
            return _context.Classifications.FirstOrDefault(c => c.ClassificationId == id.ToUpper());
        }

        public ClassificationService(HousewareContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public Response AddClassification(AddClassificationRequest model)
        {
            var response = new Response();
            try
            {
                if (GetById(model.ClassificationId) == null)
                {
                    var classification = new Classification()
                    {
                        ClassificationId = model.ClassificationId,
                        Name = model.Name,
                        ImageMenu = _imageService.UploadImage(model.ImageMenu),
                        ImageBanner = model.ImageBanner != null ? _imageService.UploadImage(model.ImageBanner) : null,
                        Story = model.Story?.ToString(),
                        Enable = model.Enable
                    };

                    _context.Classifications.Add(classification);
                    _context.SaveChanges();

                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_Exist);
                    response.SetResult("There already exists a Classification with such ClassificationId");
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

        public Response GetClassification(string id, bool? enable = null)
        {
            var response = new Response();
            try
            {
                id = id.ToUpper();
                var classification = _context.Classifications.FirstOrDefault(c => c.ClassificationId == id && (enable == null || c.Enable == enable));
                if (classification == null)
                {
                    response.SetCode(CodeTypes.Err_NotFound);
                    response.SetResult("No Classification was found for this ClassificationId");
                }
                else
                {
                    var result = new GetClassificationResponse()
                    {
                        ClassificationId = classification.ClassificationId,
                        Name = classification.Name,
                        ImageMenu = classification.ImageMenu,
                        ImageBanner = classification.ImageBanner
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

        public Response GetAllClassification(bool? enable = null)
        {
            var response = new Response();
            try
            {
                var classifications = _context.Classifications.Where(c => enable == null || c.Enable == enable).OrderBy(c => c.Sort).ToList();
                var result = new List<GetClassificationResponse>();
                foreach(var c in classifications)
                {
                    result.Add(new GetClassificationResponse()
                    {
                        ClassificationId = c.ClassificationId,
                        Name = c.Name,
                        ImageMenu = c.ImageMenu,
                        ImageBanner = c.ImageBanner
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

        public Response UpdateClassification(string id, AddClassificationRequest model)
        {
            var response = new Response();
            try
            {
                model.ClassificationId = model.ClassificationId.ToUpper();
                if (id.ToUpper() != model.ClassificationId)
                {
                    response.SetCode(CodeTypes.Err_IdNotMatch);
                    response.SetResult("ClassificationId in URL doesn't match ClassificationId in model");
                    return response;
                }

                var classification = GetById(model.ClassificationId);
                if (classification != null)
                {
                    classification.Name = model.Name;
                    
                    if(!model.ImageMenu.IsUrl || classification.ImageMenu != model.ImageMenu.Content)
                    {
                        classification.ImageMenu = _imageService.UploadImage(model.ImageMenu);
                    }

                    if (model.ImageBanner != null)
                    {
                        if (!model.ImageBanner.IsUrl || classification.ImageBanner != model.ImageBanner.Content) 
                        {
                            classification.ImageBanner = _imageService.UploadImage(model.ImageBanner);
                        } 
                    }
                    else
                    {
                        classification.ImageBanner = null;
                    }
                    classification.Story = model.Story?.ToString();
                    classification.Enable = model.Enable;

                    _context.Entry(classification).State = EntityState.Modified;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                }
                else
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("There not exists a Classification with such ClassificationId");
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

        public Response DeleteClassification(string id)
        {
            var response = new Response();
            try
            {
                var classification = GetById(id);
                if (classification != null)
                {
                    _context.Entry(classification).State = EntityState.Deleted;
                    _context.SaveChanges();
                    response.SetCode(CodeTypes.Success);
                    return response;
                }
                response.SetCode(CodeTypes.Err_NotExist);
                response.SetResult("There not exists a Classification with such ClassificationId");
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
