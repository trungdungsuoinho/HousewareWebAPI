using Houseware.WebAPI.Data;
using Houseware.WebAPI.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using System;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IClassificationService
    {
        public Reponse AddClassification(AddClassificationRequest model);
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

        public Reponse AddClassification(AddClassificationRequest model)
        {
            var reponse = new Reponse();
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
                        Story = model.Story != null ? model.Story.ToString() : null,
                        Enable = model.Enable
                    };

                    _context.Classifications.Add(classification);
                    _context.SaveChanges();

                    reponse.SetCode(CodeTypes.Success);
                }
                else
                {
                    reponse.SetCode(CodeTypes.Err_Exist);
                    reponse.SetResult("There already exists a Classification with such ClassificationId");
                }
                return reponse;
            }
            catch (Exception e)
            {
                reponse.SetCode(CodeTypes.Err_Exception);
                reponse.SetResult(e.Message);
                return reponse;
            }
        }
    }
}
