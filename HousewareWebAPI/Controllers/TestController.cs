using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
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
        private readonly IVNPayService _vNPayService;

        public TestController(IVNPayService vNPayService)
        {
            _vNPayService = vNPayService;
        }

        [HttpGet("payment")]
        public IActionResult GetProvince()
        {
            Response response = new();
            response.SetResult(_vNPayService.GeneratePaymentURL("https://www.facebook.com/", Guid.NewGuid(), 199000));
            response.SetCode(CodeTypes.Success);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
