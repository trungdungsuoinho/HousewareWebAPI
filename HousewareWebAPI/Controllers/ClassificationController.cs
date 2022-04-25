using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public ClassificationController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        /// <summary>
        /// Add a new Classification
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public IActionResult Post([FromBody] AddClassificationRequest model)
        {
            var response = _classificationService.AddClassification(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode == 999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get a Classification. API for client
        /// </summary>
        /// <param name="model"></param>
        [HttpGet("id/{id}")]
        public IActionResult GetClient([FromRoute] string id)
        {
            var response = _classificationService.GetClassification(id, true);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode == 999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get a Classification. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("admin/id/{id}/{enable?}")]
        public IActionResult GetAdmin([FromRoute] string id, bool? enable)
        {
            var response = _classificationService.GetClassification(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode == 999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Classification. API for client
        /// </summary>
        /// <param name="model"></param>
        [HttpGet("all")]
        public IActionResult GetAllClient()
        {
            var response = _classificationService.GetAllClassification(true);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode == 999) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Classification. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("admin/all/{enable?}")]
        public IActionResult GetAllAdmin([FromRoute] bool? enable)
        {
            var response = _classificationService.GetAllClassification(enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode == 999) return BadRequest(response);
            return Ok(response);
        }

        //// PUT api/<ClassificationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ClassificationController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
