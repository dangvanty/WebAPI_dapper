using WebAPI_dapper.Models;

namespace WebAPI_dapper.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }   

    }
}
