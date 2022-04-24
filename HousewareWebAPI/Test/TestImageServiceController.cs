﻿using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestImageServiceController : ControllerBase
    {
        private readonly IImageService _imageService;
        public TestImageServiceController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [AllowAnonymous]
        [HttpPost("postimageurl")]

        public IActionResult PostImage(AddImageRequest model)
        {
            var response = TestService.AddImageUrl(_imageService, model);
            return Ok(response);
        }
    }
}