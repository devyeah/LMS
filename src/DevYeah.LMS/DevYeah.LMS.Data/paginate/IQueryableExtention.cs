using System;
using System.Linq;

namespace DevYeah.LMS.Data.paginate
{
    public static class IQueryableExtention
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            result.PageCount = (int)Math.Ceiling(result.RowCount / (double)result.PageSize);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
