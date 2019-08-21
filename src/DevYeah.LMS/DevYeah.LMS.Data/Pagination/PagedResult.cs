using System.Collections.Generic;

namespace DevYeah.LMS.Data.Pagination
{
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; }

        public PagedResult() => new List<T>();
    }
}
