using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/oauth")]
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
            var response = _oAuthService.VerifyEmail(code);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
