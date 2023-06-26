using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using WebAPI_dapper.Utilities.Dtos;
using WebAPI_dapper.Filters;
using WebAPI_dapper.Helpers;
using WebAPI_dapper.Data.Models;

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connectString;
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager, IConfiguration configuration) {
            _connectString = configuration.GetConnectionString("DBSQLServer");
            _userManager = userManager;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = _userManager.FindByIdAsync(Id);
            return Ok(new ApiResponse
            {
                Data = result,
                Message= $"Get user by Id = {Id}",
                Success = true

            });
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post ([FromBody]AppUser user)
        {
            var result = await _userManager.CreateAsync(user);
            if(result.Succeeded)
                return Ok(new ApiResponse { 
                    Success = true, 
                    Message= "Create new user success" 
                });
            return BadRequest(new ApiResponse { 
                Success = false,
                Message = "Create fail new user" 
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required]Guid id, [FromBody] AppUser user )
        {
            user.Id = id;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Update success user at ID = {id}"
                });
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Update fail user at ID = {id}"
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id )
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"delete a user success at id {id}"
                });
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"delete a user fail at id {id}"
            });
        }

        [HttpGet]
        [ClaimRequirement(FunctionCode.SYSTEM_USER,ActionCode.VIEW)]
        public async Task<IActionResult> Get()
        {
            using(var conn = new SqlConnection(_connectString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var parameters = new DynamicParameters();
                string GetUserAll = "GET_USER_ALL";
                var result = await conn.QueryAsync<AppUser>(GetUserAll, parameters, null, null, System.Data.CommandType.StoredProcedure);
               
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Get all user",
                    Data = result.ToList()
                });
            }
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Get(string? keyword, int pageIndex, int pageSize)
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

                string GetUserPaging = "Get_User_All_Paging";
                var result = await conn.QueryAsync<AppUser>(GetUserPaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = paramaters.Get<int>("@totalRow");

                var pageResult = new PagedResult<AppUser>
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalRow = totalRow,
                    PageSize = pageSize
                };
                return Ok(pageResult);
            }
        }
        [HttpPut("{id}/{roleName}/assign-to-roles")]
        public async Task<IActionResult> AssignToRoles([Required] Guid id, [Required] string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var conn = new SqlConnection(_connectString))
            {
                await conn.OpenAsync();
                var normalizedName = roleName.ToUpper();
                var roleId = await conn.ExecuteScalarAsync<Guid?>($"SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @{nameof(normalizedName)}", new { normalizedName });
                if (!roleId.HasValue)
                {
                    roleId = Guid.NewGuid();
                    await conn.ExecuteAsync($"INSERT INTO [AspNetRoles]([Id],[Name], [NormalizedName]) VALUES(@{nameof(roleId)},@{nameof(roleName)}, @{nameof(normalizedName)})",
                       new { roleName, normalizedName });
                }


                await conn.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) " +
                    $"INSERT INTO [AspNetUserRoles]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})",
                    new { userId = user.Id, roleId });
                return Ok();
            }
        }

        [HttpDelete("{id}/{roleName}/remove-roles")]
        [ValidateModel]
        public async Task<IActionResult> RemoveRoleToUser([Required] Guid id, [Required] string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.OpenAsync();
                var roleId = await connection.ExecuteScalarAsync<Guid?>("SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
                if (roleId.HasValue)
                    await connection.ExecuteAsync($"DELETE FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}", new { userId = user.Id, roleId });
                return Ok();
            }
        }
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var model = await _userManager.GetRolesAsync(user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = $"Get all roles of user having id : {id}",
                Data = model
            });
        }
    }
}
