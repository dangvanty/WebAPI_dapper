using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_dapper.Data.Models;
using WebAPI_dapper.Utilities.Dtos;

namespace WebAPI_dapper.Data.Interfaces
{
    public interface IFunctionResponsitory
    {
        Task<IEnumerable<Function>> GetAllAsync();
        Task<Function> GetByIdAsync(string Id);
        Task<PagedResult<Function>> GetPagingAsync(string? keyword, int pageIndex, int pageSize);
        Task CreateAsync(Function function);
        Task EditAsync(Function function);
        Task DeleteAsync(string id);
    }
}
