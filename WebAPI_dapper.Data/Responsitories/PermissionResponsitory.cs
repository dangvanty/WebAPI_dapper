using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_dapper.Data.ViewModel;
using Microsoft.Extensions.Configuration;
using Dapper;
using WebAPI_dapper.Data.Interfaces;

namespace WebAPI_dapper.Data.Responsitories
{
    public class PermissionResponsitory : IPermissionResponsitory
    {
        private readonly string _connectString;
        public PermissionResponsitory(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("DBSQLServer");
        }

        public async Task<IEnumerable<FunctionActionViewModel>> GetAllWithPermission()
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();

                var result = await conn.QueryAsync<FunctionActionViewModel>("Get_Function_WithActions", null, null, null, System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
        public async Task<IEnumerable<PermissionViewModel>> GetAllRolePermissions(Guid? role)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@roleId", role);

                var result = await conn.QueryAsync<PermissionViewModel>("Get_Permission_ByRoleId", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task SavePermissions(Guid role, List<PermissionViewModel> permissions)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var dt = new DataTable();
                dt.Columns.Add("RoleId", typeof(Guid));
                dt.Columns.Add("FunctionId", typeof(string));
                dt.Columns.Add("ActionId", typeof(string));
                foreach (var item in permissions)
                {
                    dt.Rows.Add(role, item.FunctionId, item.ActionId);
                }
                var paramaters = new DynamicParameters();
                paramaters.Add("@roleId", role);
                paramaters.Add("@permissions", dt.AsTableValuedParameter("dbo.Permission"));
                await conn.ExecuteAsync("Create_Permission", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                
            }
        }
        public async Task<IEnumerable<FunctionViewModel>> GetAllFunctionByPermission(Guid userID)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@userId", userID);

                var result = await conn.QueryAsync<FunctionViewModel>("Get_Function_ByPermission", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
