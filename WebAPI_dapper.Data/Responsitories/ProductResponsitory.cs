using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Utilities.Dtos;

namespace WebAPI_dapper.Data.Responsitories
{
    public class ProductResponsitory
    {
        private readonly string _connectString;
        public ProductResponsitory(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("DBSQLServer");
        }
        public async Task<IEnumerable<Product>> GetAllAsync(string culture)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                //string querySelectProduct = @"SELECT Id, Sku,Price,DiscountPrice,
                //                            IsActive,RateCount,RateTotal 
                //                            from Products";

                var paramaters = new DynamicParameters();
                paramaters.Add("@language", culture);
                string storedSelectAllPro = "Get_Product_All";

                var result = await conn.QueryAsync<Product>(storedSelectAllPro, paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
        public async Task<Product> GetByIdAsync (int id, string culture)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", culture);

                string storedFindProById = "Find_Product_By_Id";
                var result = await conn.QueryAsync<Product>(storedFindProById, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                
                return result.Single();

            }
        }
        public async Task<PagedResult<Product>> GetPaggingAsync(string? keyword, int categoryId, int pageIndex, int pageSize,string culture)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@categoryId", categoryId);

                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@language", culture);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string GetProductPaging = "Get_Product_All_Paging";
                var result = await conn.QueryAsync<Product>(GetProductPaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = paramaters.Get<int>("@totalRow");
                var pageResult = new PagedResult<Product>
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalRow = totalRow,
                    PageSize = pageSize
                };
                return pageResult;
            }
        }
        public async Task<int> CreateAsync(Product product, string culture)
        {
            int newId = 0;
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@name", product.Name);
                paramaters.Add("@seoDescription", product.SeoDescription);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@content", product.Content);
                paramaters.Add("@seoAlias", product.SeoAlias);
                paramaters.Add("@seoTitle", product.SeoTitle);
                paramaters.Add("@seoKeyword", product.SeoKeyword);
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActice", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@imageList", product.ImageList);
                paramaters.Add("@language", culture);
                paramaters.Add("@categoryIds", product.CategoryIds);
                paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string createProduct = "Create_Product";
                var result = await conn.ExecuteAsync(createProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);


                newId = paramaters.Get<int>("@id");

            }
            return newId;
        }
        public async Task EditAsync(int id, Product product, string culture)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@name", product.Name);
                paramaters.Add("@seoDescription", product.SeoDescription);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@content", product.Content);
                paramaters.Add("@seoAlias", product.SeoAlias);
                paramaters.Add("@seoTitle", product.SeoTitle);
                paramaters.Add("@seoKeyword", product.SeoKeyword);
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActice", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@imageList", product.ImageList);
                paramaters.Add("@language", culture);
                paramaters.Add("@categoryIds", product.CategoryIds);
                string editProduct = "Edit_Product";
                await conn.ExecuteAsync(editProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                string deleteProduct = "Delete_Product_By_Id";

                await conn.ExecuteAsync(deleteProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);

            }
        }
        public async Task<List<int>> CreateManyAsync(List<Product> products,string culture)
        {
            List<int> ids = new List<int>();
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                foreach (var product in products)
                {


                    var paramaters = new DynamicParameters();
                    paramaters.Add("@name", product.Name);
                    paramaters.Add("@seoDescription", product.SeoDescription);
                    paramaters.Add("@description", product.Description);
                    paramaters.Add("@content", product.Content);
                    paramaters.Add("@seoAlias", product.SeoAlias);
                    paramaters.Add("@seoTitle", product.SeoTitle);
                    paramaters.Add("@seoKeyword", product.SeoKeyword);
                    paramaters.Add("@sku", product.Sku);
                    paramaters.Add("@price", product.Price);
                    paramaters.Add("@isActice", product.IsActive);
                    paramaters.Add("@imageUrl", product.ImageUrl);
                    paramaters.Add("@imageList", product.ImageList);
                    paramaters.Add("@language", culture);
                    paramaters.Add("@categoryIds", product.CategoryIds);
                    paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                    string createProduct = "Create_Product";
                    var result= await conn.ExecuteAsync(createProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);


                    ids.Add(paramaters.Get<int>("@id"));
                }
            }
            return ids;
        }
    }
}
