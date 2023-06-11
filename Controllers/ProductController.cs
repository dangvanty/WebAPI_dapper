using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml.Linq;
using WebAPI_dapper.Dtos;
using WebAPI_dapper.Extensions;
using WebAPI_dapper.Helpers;
using WebAPI_dapper.Models;
using WebAPI_dapper.Resources;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_dapper.Controllers
{
    [Route("api/{culture}/[controller]")]
    [ApiController]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProductController : ControllerBase
    {
        private readonly string _connectString;
        private readonly ILogger<ProductController> _logger;
        private readonly IStringLocalizer<ProductController> _localizer;
        private readonly LocalService _localService;
        public ProductController(IConfiguration configuration, ILogger<ProductController> logger, IStringLocalizer<ProductController> localizer, LocalService localService)
        {

            _connectString = configuration.GetConnectionString("DBSQLServer");
            _logger = logger;
            _localizer = localizer;
            _localService = localService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            //string text = _localizer["dog"];
            //string text1 = _localService.GetLocalizedHtmlString("ForgotPassword");
            //_logger.LogError("Test product controller");


            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                //string querySelectProduct = @"SELECT Id, Sku,Price,DiscountPrice,
                //                            IsActive,RateCount,RateTotal 
                //                            from Products";

                var paramaters = new DynamicParameters();
                paramaters.Add("@language", CultureInfo.CurrentCulture.Name);
                string storedSelectAllPro = "Get_Product_All";
                
                var result = await conn.QueryAsync<Product>(storedSelectAllPro, paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiResponse
                {
                    Data = result,
                    Message = "Success for getting all",
                    Success = true
                });
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", CultureInfo.CurrentCulture.Name);

                string storedFindProById = "Find_Product_By_Id";
                var result = await conn.QueryAsync<Product>(storedFindProById, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return Ok(new ApiResponse
                {
                    Message = $"Find Product by ID ={id} ",
                    Success = true,
                    Data = result.Single()
                });

            }
        }

        [HttpGet("Paging", Name = "GetPagging")]
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
                paramaters.Add("@language", CultureInfo.CurrentCulture.Name);
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

        // POST api/<ProductController>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Product product)
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
                paramaters.Add("@language", CultureInfo.CurrentCulture.Name);
                paramaters.Add("@categoryIds", product.CategoryIds);
                paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                string createProduct = "Create_Product";
                var result = await conn.ExecuteAsync(createProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);


                newId = paramaters.Get<int>("@id");

            }
            return Ok(new ApiResponse
            {
                Message = "Created success",
                Success = true,
                Data = $"Product id {newId} was created!"
            });
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] Product product)
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
                paramaters.Add("@language", CultureInfo.CurrentCulture.Name);
                paramaters.Add("@categoryIds", product.CategoryIds);
                string editProduct = "Edit_Product";
                await conn.ExecuteAsync(editProduct, paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return NoContent();

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
