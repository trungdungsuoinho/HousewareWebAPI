using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public ClassificationsController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        /// <summary>
        /// Get a Classification. API for client
        /// </summary>
        /// <param name="model"></param>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            var response = _classificationService.GetClassification(id, true);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Classification. API for client
        /// </summary>
        /// <param name="model"></param>
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _classificationService.GetAllClassification(true);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}