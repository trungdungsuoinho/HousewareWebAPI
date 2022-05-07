using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using System;

namespace HousewareWebAPI.Test
{
    public class TestService
    {
        public static Response Test()
        {
            var reponse = new Response();
            int a = 0;
            if (a == 0)
            {
                throw new Exception("Oke chưa!");
            }
            return reponse;
        }

        public static Response AddImageUrl(IImageService imageService, ImageInput model)
        {
            var reponse = new Response();
            var image = imageService.UploadImage(model);
            if (image != string.Empty)
            {
                reponse.SetCode(CodeTypes.Success);
                reponse.SetResult(image);
            }
            return reponse;
        }
    }
}
