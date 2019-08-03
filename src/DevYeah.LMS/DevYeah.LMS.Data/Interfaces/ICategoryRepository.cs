using System.Collections.Generic;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        int CountByName(string name);
        IEnumerable<Category> FindAllCategories();
    }
}
