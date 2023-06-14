using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using WebAPI_dapper.Dtos;
using WebAPI_dapper.Helpers;
using WebAPI_dapper.Models;

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly string _connectString;
        public FunctionController(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("DBSQLServer");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            using(var conn = new SqlConnection(_connectString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();

                string getAllFunct = "GET_ALL_FUNCTION";
                var result = await conn.QueryAsync<Function>(getAllFunct, paramaters, null,null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiResponse
                {
                    Data = result,
                    Success = true,
                    Message = "Get all function"
                });
            }
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@id", Id);
               
                string storedFunctById = "Find_Function_By_Id";
                var result = await conn.QueryAsync<Function>(storedFunctById, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return Ok(new ApiResponse
                {
                    Message = $"Find Function by ID ={Id} ",
                    Success = true,
                    Data = result.Single()
                });

            }
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging (string? keyword, int pageIndex,int pageSize)
        {
            using(var conn = new SqlConnection(_connectString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow",dbType:System.Data.DbType.Int32, direction:System.Data.ParameterDirection.Output);

                string GetFunctPaging = "Get_Function_All_Paging";
                var result = await conn.QueryAsync<Function>(GetFunctPaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = paramaters.Get<int>("@totalRow");

                var pageResult = new PagedResult<Function>
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalRow = totalRow,
                    PageSize = pageSize
                };
                return Ok(pageResult);
            }    
        }

        // POST: api/Function
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Function function)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", function.Id);
                paramaters.Add("@name", function.Name);
                paramaters.Add("@url", function.Url);
                paramaters.Add("@parentId", function.ParentId);
                paramaters.Add("@sortOrder", function.SortOrder);
                paramaters.Add("@cssClass", function.CssClass);
                paramaters.Add("@isActive", function.IsActive);

                await conn.ExecuteAsync("Create_Function", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return Ok();
            }
        }
        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required] Guid id, [FromBody] Function function)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", function.Id);
                paramaters.Add("@name", function.Name);
                paramaters.Add("@url", function.Url);
                paramaters.Add("@parentId", function.ParentId);
                paramaters.Add("@sortOrder", function.SortOrder);
                paramaters.Add("@cssClass", function.CssClass);
                paramaters.Add("@isActive", function.IsActive);

                await conn.ExecuteAsync("Update_Function", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return Ok();
            }
        }


        // DELETE: api/Function/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                await conn.ExecuteAsync("Delete_Function_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return Ok();
            }
        }

    }
}
