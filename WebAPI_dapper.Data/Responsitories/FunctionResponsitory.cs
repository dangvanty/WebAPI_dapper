using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebAPI_dapper.Data.Interfaces;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Utilities.Dtos;

namespace WebAPI_dapper.Data.Responsitories
{
    public class FunctionResponsitory : IFunctionResponsitory
    {
        private readonly string _connectString;
        public FunctionResponsitory(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("DBSQLServer");
        }
        public async Task<IEnumerable<Function>> GetAllAsync()
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();

                string getAllFunct = "GET_ALL_FUNCTION";
                var result = await conn.QueryAsync<Function>(getAllFunct, paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
        public async Task<Function> GetByIdAsync(string Id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@id", Id);

                string storedFunctById = "Find_Function_By_Id";
                var result = await conn.QueryAsync<Function>(storedFunctById, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();

            }
        }
        public async Task<PagedResult<Function>> GetPagingAsync(string? keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string GetFunctPaging = "Get_Function_All_Paging";
                var result = await conn.QueryAsync<Function>(GetFunctPaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = paramaters.Get<int>("@totalRow");

                return new PagedResult<Function>
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalRow = totalRow,
                    PageSize = pageSize
                };

            }
        }
        public async Task CreateAsync(Function function)
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
            }
        }
        public async Task EditAsync(Function function)
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
            }
        }
        public async Task DeleteAsync(string id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                await conn.ExecuteAsync("Delete_Function_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
