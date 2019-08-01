using System.Collections.Generic;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Category> FindAllCategories() => FindAll(cat => true);

        public int CountByName(string name) => Count(x => EF.Functions.Like(x.Name, $"%{name}"));
    }
}
