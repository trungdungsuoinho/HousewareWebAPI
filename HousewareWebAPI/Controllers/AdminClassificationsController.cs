using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/admin/classifications")]
    [ApiController]
    public class AdminClassificationsController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public AdminClassificationsController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        /// <summary>
        /// Get a Classification. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("{id}/{enable?}")]
        public IActionResult Get([FromRoute] string id, bool? enable)
        {
            var response = _classificationService.GetClassAdmin(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Classification. API for admin
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("all/{enable?}")]
        public IActionResult GetAll([FromRoute] bool? enable)
        {
            var response = _classificationService.GetAllClassAdmin(enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add a new Classification
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public IActionResult Post([FromBody] AddClassAdminRequest model)
        {
            var response = _classificationService.AddClassAdmin(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update a Classification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id,[FromBody] AddClassAdminRequest model)
        {
            var response = _classificationService.UpdateClassAdmin(id, model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Detele a Classification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var response = _classificationService.DeleteClassAdmin(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify Sort of Classifications
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("modifysort")]
        public IActionResult ModifySort([FromRoute] List<string> ids)
        {
            var response = _classificationService.ModifySort(ids);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
