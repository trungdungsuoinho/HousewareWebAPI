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
    [Route("api/v1/admin/products")]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get a Product. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("{id}/{enable?}")]
        public IActionResult Get([FromRoute] string id, bool? enable)
        {
            var response = _productService.GetProAdmin(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get all Product. API for admin
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("all/{enable?}")]
        public IActionResult GetAll([FromRoute] bool? enable)
        {
            var response = _productService.GetAllProAdmin(enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Product by CategoryId. API for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpGet("class/{id}/{enable?}")]
        public IActionResult GetByClassId([FromRoute] string id, bool? enable)
        {
            var response = _productService.GetProAdminByCatId(id, enable);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add a new Product
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public IActionResult Post([FromBody] AddProAdminRequest model)
        {
            var response = _productService.AddProAdmin(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update a Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] AddProAdminRequest model)
        {
            var response = _productService.UpdateProAdmin(id, model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Detele a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var response = _productService.DeleteProAdmin(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Modify Sort of Products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("modifysort")]
        public IActionResult ModifySort([FromBody] ModifySortProAdminRequest model)
        {
            var response = _productService.ModifySort(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
