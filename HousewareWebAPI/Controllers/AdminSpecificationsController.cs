using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/admin/specifications")]
    [ApiController]
    public class AdminSpecificationsController : ControllerBase
    {
        private readonly ISpecificationService _specificationService;

        public AdminSpecificationsController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }

        /// <summary>
        /// Get a Specification. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            var response = _specificationService.GetSpecAdmin(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Specification. API for admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _specificationService.GetAllSpecAdmin();
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add a new Specification
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public IActionResult Post([FromBody] AddSpecAdminRequest model)
        {
            var response = _specificationService.AddSpecAdmin(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update a Specification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] AddSpecAdminRequest model)
        {
            var response = _specificationService.UpdateSpecAdmin(id, model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Detele a Specification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var response = _specificationService.DeleteSpecAdmin(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify Sort of Specification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("modifysort")]
        public IActionResult ModifySort([FromBody] ModifySortSpecAdminRequest model)
        {
            var response = _specificationService.ModifySort(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
