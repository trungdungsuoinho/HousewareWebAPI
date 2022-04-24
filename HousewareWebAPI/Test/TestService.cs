using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using System;

namespace HousewareWebAPI.Test
{
    public class TestService
    {
        public static Reponse Test()
        {
            var reponse = new Reponse();
            int a = 0;
            if (a == 0)
            {
                throw new Exception("Oke chưa!");
            }
            return reponse;
        }

        public static Reponse AddImageUrl(IImageService imageService, AddImageRequest model)
        {
            var reponse = new Reponse();
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
