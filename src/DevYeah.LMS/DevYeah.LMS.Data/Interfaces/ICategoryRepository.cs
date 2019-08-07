using System;
using System.Collections.Generic;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        bool IsExistedName(string name);
        bool IsExisted(Guid key);
        IEnumerable<Category> FindAllCategories();
    }
}
