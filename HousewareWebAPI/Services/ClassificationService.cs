using Houseware.WebAPI.Data;
using Houseware.WebAPI.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
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
    }
    public class ClassificationService : IClassificationService
    {
        private readonly HousewareContext _context;
        private readonly IImageService _imageService;

        private bool CheckExist(string id)
        {
            return _context.Classifications.FirstOrDefault(c => c.ClassificationId == id) != null;
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
                if (!CheckExist(model.ClassificationId))
                {
                    var classification = new Classification()
                    {
                        ClassificationId = model.ClassificationId.ToUpper(),
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
                var classifications = _context.Classifications.Where(c => enable == null || c.Enable == enable).ToList();
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
    }
}
