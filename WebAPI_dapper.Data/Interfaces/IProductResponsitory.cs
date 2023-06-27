using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Utilities.Dtos;

namespace WebAPI_dapper.Data.Interfaces
{
    public interface IProductResponsitory
    {
        Task<IEnumerable<Product>> GetAllAsync(string culture);
        Task<Product> GetByIdAsync(int id, string culture);
        Task<PagedResult<Product>> GetPaggingAsync(string? keyword, int categoryId, int pageIndex, int pageSize, string culture);
        Task<int> CreateAsync(Product product, string culture);
        Task EditAsync(int id, Product product, string culture);
        Task DeleteAsync(int id);
        Task<List<int>> CreateManyAsync(List<Product> products, string culture);

    }
}
