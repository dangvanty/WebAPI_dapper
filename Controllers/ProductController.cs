using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.WebSockets;
using WebAPI_dapper.Dtos;
using WebAPI_dapper.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string _connectString;
        public ProductController(IConfiguration configuration) {

            _connectString = configuration.GetConnectionString("DBSQLServer");
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            using(var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                //string querySelectProduct = @"SELECT Id, Sku,Price,DiscountPrice,
                //                            IsActive,RateCount,RateTotal 
                //                            from Products";
                string storedSelectAllPro = "Get_Product_All";
;              var result = await conn.QueryAsync<Product>(storedSelectAllPro, null, null, null, System.Data.CommandType.StoredProcedure);

                return result;
            }    
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                string storedFindProById = "Find_Product_By_Id";
                var result = await conn.QueryAsync<Product>(storedFindProById, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();

            }
        }

        [HttpGet("Paging",Name ="GetPagging")]
        public async Task<PagedResult<Product>> GetPagging(string? keyword, int categoryId, int pageIndex, int pageSize)
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

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string GetProductPaging = "Get_Product_All_Paging";
                var result = await conn.QueryAsync<Product>(GetProductPaging, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                int totalPage = paramaters.Get<int>("@totalRow");
                var pageResult =  new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    PageIndex = pageIndex,
                    TotalPage = totalPage,
                    PageSize = pageSize
                };
                return pageResult;
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<int> Post([FromBody] Product product)
        {
            int newId = 0;
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActice", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);

                paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string createProduct = "Create_Product";
                var result = await conn.ExecuteAsync(createProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);


                newId = paramaters.Get<int>("@id");

            }
            return newId;   
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Product product)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActice", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);

                string editProduct = "Edit_Product";
                await conn.ExecuteAsync(editProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);


            }
               
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
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
    }
}
