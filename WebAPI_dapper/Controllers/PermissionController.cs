using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebAPI_dapper.Data.Interfaces;
using WebAPI_dapper.Data.ViewModel;
using WebAPI_dapper.Extensions;

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionResponsitory _permissionResponsitory;
        public PermissionController(IPermissionResponsitory permissionResponsitory)
        {
            _permissionResponsitory = permissionResponsitory;
        }
        [HttpGet("function-actions")]
        public async Task<IActionResult> GetAllWithPermission()
        {
            var result = await _permissionResponsitory.GetAllWithPermission();
            return Ok(result);

        }

        [HttpGet("{role}/role-permissions")]
        public async Task<IActionResult> GetAllRolePermissions(Guid? role)
        {
            var result = await _permissionResponsitory.GetAllRolePermissions(role);
            return Ok(result);
            
        }

        [HttpPost("{role}/save-permissions")]
        public async Task<IActionResult> SavePermissions(Guid role, [FromBody] List<PermissionViewModel> permissions)
        {
            await _permissionResponsitory.SavePermissions(role, permissions);
            return Ok();
          
        }

        [HttpGet("functions-view")]
        public async Task<IActionResult> GetAllFunctionByPermission()
        {
            var result = await _permissionResponsitory.GetAllFunctionByPermission(User.GetUserId());
            return Ok(result);
        }
    }
}
