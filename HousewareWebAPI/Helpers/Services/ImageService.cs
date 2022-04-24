using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IImageService
    {
        public string UploadImage(AddImageRequest model);
    }

    public class ImageService : IImageService
    {
        private readonly AppSettings _appSettings;
        private Cloudinary _cloudinary;

        public ImageService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            InitCloudinary();
        }

        private void InitCloudinary()
        {
            var account = new Account(_appSettings.ImageCloudName, _appSettings.ImageAPIKey, _appSettings.ImageAPISecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        private static Stream Base64ToStream(string base64)
        {
            return new MemoryStream(Convert.FromBase64String(base64));
        }

        private static bool CheckImageUrl(string url)
        {
            var result = false;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                result = response.StatusCode == HttpStatusCode.OK && response.Headers["Content-Type"].StartsWith("image/");
                response.Dispose();
            }
            return result;
        }

        public string UploadImage(AddImageRequest model)
        {
            if (!string.IsNullOrEmpty(model.Content))
            {
                if (model.IsUrl)
                {
                    if (CheckImageUrl(model.Content))
                    {
                        return model.Content;
                    }
                    throw new Exception("The url of the image is not valid");
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Name))
                    {
                        model.Name = RandomString.GetRandomString(12);
                    }

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(model.Name, Base64ToStream(model.Content))
                    };

                    var uploadResult = _cloudinary.Upload(uploadParams);
                    if (uploadResult.ResourceType.ToLower() == "image")
                    {
                        return uploadResult.Url.ToString();
                    }

                    throw new Exception("Image upload failed!");
                }
            }
            throw new Exception("Image is null!");
        }
    }
}
