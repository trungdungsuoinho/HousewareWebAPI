using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IOAuthService _oAuthService;

        public OAuthController(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        [HttpGet("login")]
        public IActionResult GenURILogin()
        {
            var response = _oAuthService.GenOAuthURI();
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] string code)
        {
            var response = _oAuthService.GetOAuthInfo(code);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
