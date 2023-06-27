using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using WebAPI_dapper.Data.Interfaces;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Helpers;
using WebAPI_dapper.Utilities.Dtos;

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly IFunctionResponsitory _functionResponsitory;
        public FunctionController(IFunctionResponsitory functionResponsitory)
        {
            _functionResponsitory = functionResponsitory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var result = await _functionResponsitory.GetAllAsync();

            return Ok(new ApiResponse
            {
                Data = result,
                Success = true,
                Message = "Get all function"
            });

        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _functionResponsitory.GetByIdAsync(Id);
            return Ok(new ApiResponse
            {
                Message = $"Find Function by ID ={Id} ",
                Success = true,
                Data = result
            });


        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string? keyword, int pageIndex, int pageSize)
        {
            var result = await _functionResponsitory.GetPagingAsync(keyword, pageIndex, pageSize);
            return Ok(new ApiResponse
            {
                Message = $"Get function pagging ",
                Success = true,
                Data = result
            });
        }

        // POST: api/Function
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Function function)
        {
           await _functionResponsitory.CreateAsync(function);
            return Ok(new ApiResponse
            {
                Message = $"Create new function success ",
                Success = true,
            });
        }
        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required] Guid id, [FromBody] Function function)
        {
            await _functionResponsitory.EditAsync(function);
            return Ok(new ApiResponse
            {
                Message = $"edit function success ",
                Success = true,
            });
        }


        // DELETE: api/Function/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           await _functionResponsitory.DeleteAsync(id);
            return Ok(new ApiResponse
            {
                Message = $"delete Function by ID ={id} ",
                Success = true
            });
        }

    }
}
