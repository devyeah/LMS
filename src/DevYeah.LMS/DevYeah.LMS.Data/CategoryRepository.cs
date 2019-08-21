using System;
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

        public bool IsExistedName(string name)
        {
            var total = Count(x => EF.Functions.Like(x.Name, $"%{name}"));
            return total > 0;
        }

        public bool IsExisted(Guid key)
        {
            var total = Count(x => x.Id == key);
            return total > 0;
        }
    }
}
