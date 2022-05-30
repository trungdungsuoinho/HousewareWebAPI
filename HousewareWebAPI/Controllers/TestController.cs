using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IGHNService _gHNService;

        public TestController(IGHNService gHNService)
        {
            _gHNService = gHNService;
        }

        [HttpGet("province")]
        public IActionResult GetProvince()
        {
            var response = _gHNService.GetProvince();
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
