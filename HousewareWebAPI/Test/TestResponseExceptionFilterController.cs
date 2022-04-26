using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HousewareWebAPI.Test
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestResponseExceptionFilterController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("raw-exception")]

        public IActionResult RawException()
        {
            var response = TestService.Test();
            if (response == null)
            {
                throw new Exception("Oke rồi đấy");
            }
            return Ok(response);
        }
    }
}
