using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_dapper.Data.ViewModel;

namespace WebAPI_dapper.Data.Interfaces
{
    public interface IAttributeRepository
    {
        Task<List<AttributeViewModel>> GetAll(string culture);

        Task<AttributeViewModel> GetById(int id, string culture);

        Task Add(string culture, AttributeViewModel attribute);

        Task Update(int id, string culture, AttributeViewModel attribute);

        Task Delete(int id);
    }
}
