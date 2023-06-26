using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Data.SqlClient;
using System.Globalization;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Data.Responsitories;
using WebAPI_dapper.Extensions;
using WebAPI_dapper.Helpers;
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
        private readonly ProductResponsitory _productResponsitory;
        public ProductController(IConfiguration configuration, ILogger<ProductController> logger, IStringLocalizer<ProductController> localizer, LocalService localService)
        {

            _connectString = configuration.GetConnectionString("DBSQLServer");
            _logger = logger;
            _localizer = localizer;
            _localService = localService;
            _productResponsitory = new ProductResponsitory(configuration);
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            //string text = _localizer["dog"];
            //string text1 = _localService.GetLocalizedHtmlString("ForgotPassword");
            //_logger.LogError("Test product controller");


            var result = await _productResponsitory.GetAllAsync(CultureInfo.CurrentCulture.Name);

            return Ok(new ApiResponse
            {
                Data = result,
                Message = "Success for getting all ",
                Success = true
            });
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productResponsitory.GetByIdAsync(id, CultureInfo.CurrentCulture.Name);
            return Ok(new ApiResponse
            {
                Message = $"Find Product by ID ={id} ",
                Success = true,
                Data = result
            });
        }

        [HttpGet("Paging", Name = "GetPagging")]
        public async Task<IActionResult> GetPagging(string? keyword, int categoryId, int pageIndex, int pageSize)
        {
            var result = await _productResponsitory.GetPaggingAsync(keyword, categoryId, pageIndex, pageSize, CultureInfo.CurrentCulture.Name);
            return Ok(new ApiResponse
            {
                Message = $"Get pagging success",
                Success = true,
                Data = result
            });
        }

        // POST api/<ProductController>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            var newId = await _productResponsitory.CreateAsync(product, CultureInfo.CurrentCulture.Name);
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
            await _productResponsitory.EditAsync(id, product, CultureInfo.CurrentCulture.Name);
            return Ok(new ApiResponse
            {
                Message = "Edit success",
                Success = true,
                Data = $"Product id {id} was edit!"
            });

        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productResponsitory.DeleteAsync(id);
            return Ok(new ApiResponse
            {
                Message = "delete success",
                Success = true,
                Data = $"Product id {id} was dêleted!"
            });
        }
        // POST create many
        [HttpPost("CreateMany")]
        public async Task<IActionResult> CreateMany([FromBody] List<Product> products)
        {
            var ids = await _productResponsitory.CreateManyAsync(products, CultureInfo.CurrentCulture.Name);
            return Ok(new ApiResponse
            {
                Message = "create many product success",
                Success = true,
                Data = $"Products id ind [ {String.Join(",",ids)}] were created "
            });
        }
    }
}
