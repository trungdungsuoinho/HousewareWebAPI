using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/admin/categories")]
    [ApiController]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get a Category. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("{id}/{enable?}")]
        public IActionResult Get([FromRoute] string id, bool? enable)
        {
            var response = _categoryService.GetCatAdmin(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Category. API for admin
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("all/{enable?}")]
        public IActionResult GetAll([FromRoute] bool? enable)
        {
            var response = _categoryService.GetAllCatAdmin(enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Categories by ClassificationId. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("class/{id}/{enable?}")]
        public IActionResult GetByClassId([FromRoute] string id, bool? enable)
        {
            var response = _categoryService.GetCatAdminByClassId(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add a new Category
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public IActionResult Post([FromBody] AddCatAdminRequest model)
        {
            var response = _categoryService.AddCatAdmin(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update a Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] AddCatAdminRequest model)
        {
            var response = _categoryService.UpdateCatAdmin(id, model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Detele a Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var response = _categoryService.DeleteCatAdmin(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
