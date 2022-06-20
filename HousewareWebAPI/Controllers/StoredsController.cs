using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/storeds")]
    [ApiController]
    public class StoredsController : ControllerBase
    {
        private readonly IStoredService _storedService;

        public StoredsController(IStoredService storedService)
        {
            _storedService = storedService;
        }

        [HttpPost("get")]
        public IActionResult GetStored([FromBody] StoredRequest model)
        {
            var response = _storedService.GetStored(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("import")]
        public IActionResult ImportIntoStored([FromBody] ChangeStoredRequest model)
        {
            var response = _storedService.ImportStored(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("export")]
        public IActionResult ExportFromStored([FromBody] ChangeStoredRequest model)
        {
            var response = _storedService.ExportStored(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
