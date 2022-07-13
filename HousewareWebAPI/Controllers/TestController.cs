using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IVNPayService _vNPayService;
        private readonly ITwilioService _twilioService;
        private readonly ISendGridService _sendGridService;

        public TestController(IVNPayService vNPayService, ITwilioService twilioService, ISendGridService sendGridService)
        {
            _vNPayService = vNPayService;
            _twilioService = twilioService;
            _sendGridService = sendGridService;
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

        [HttpGet("sendsms")]
        public IActionResult SendSMS()
        {
            Response response = new();
            response.SetResult(_twilioService.SendSMS("+84334071056", "Hello Trung Dũng"));
            response.SetCode(CodeTypes.Success);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("verify")]
        public IActionResult Verify()
        {
            Response response = new();
            response.SetResult(_twilioService.SendVerification("0334071056"));
            response.SetCode(CodeTypes.Success);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("sendmail")]
        public IActionResult SendMail()
        {
            Response response = new();
            _sendGridService.SendMail();
            response.SetCode(CodeTypes.Success);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
