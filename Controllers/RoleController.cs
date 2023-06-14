using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class RoleController : ControllerBase
    {
        private readonly string _connectString;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(IConfiguration configuration, RoleManager<AppRole> roleManager ) {
            _connectString = configuration.GetConnectionString("DBSQLServer");
            _roleManager = roleManager;
        
        }


        // GET api/Role
        [HttpGet]
        public async Task<IActionResult> Get() {

            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
              
                var paramaters = new DynamicParameters();

                string storedGetRoleAll = "GET_ALL_ROLE";

                var result = await conn.QueryAsync<AppRole>(storedGetRoleAll, paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiResponse
                {
                    Data = result,
                    Message = "Success for getting all role",
                    Success = true
                });
            }

        }

        // GET api/role/5
        [HttpGet("{Id}",Name ="Get")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var result = await _roleManager.FindByIdAsync(Id.ToString());
            if (result == null)
                return NotFound(new ApiResponse
                {
                    Data = result,
                    Message = $"Not found role at id {Id}",
                    Success = false
                });
            return Ok(new ApiResponse
            {
                Data = result,
                Message = "Success for getting role",
                Success = true
            });
        }
        [HttpGet("paging",Name ="GetPaging")]
        public async Task<IActionResult> GetPaging(string? keyword,int pageIndex,int pageSize)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);

                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string GetRolePaging = "Get_Role_All_Paging";
                var result = await conn.QueryAsync<AppRole>(GetRolePaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = paramaters.Get<int>("@totalRow");

                var pageResult = new PagedResult<AppRole>
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalRow = totalRow,
                    PageSize = pageSize
                };
                return Ok(pageResult);
            }    
        }

        // POST: api/Role
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] AppRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message= "Create a role success"
                });
            }
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Create a role fail"
            });
        }

        //PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required] Guid id, [FromBody] AppRole role)
        {
            role.Id = id;
            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"update a role success at id {id}"
                });
            }
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"update a role fail at id {id}"
            });
        }

        // DELETE api/ Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);   
            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded)
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"delete a role success at id {id}"
                });
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"delete a role fail at id {id}"
            });
        }
    }
}
