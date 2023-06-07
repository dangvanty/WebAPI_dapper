using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
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
                string querySelectProduct = @"SELECT Id, Sku,Price,DiscountPrice,
                                            IsActive,RateCount,RateTotal 
                                            from Products";
;              var result = await conn.QueryAsync<Product>(querySelectProduct, null, null, null, System.Data.CommandType.Text);

                return result;
            }    
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
