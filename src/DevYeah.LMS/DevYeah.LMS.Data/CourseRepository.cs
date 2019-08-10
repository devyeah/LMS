using System;
using System.Collections.Generic;
using System.Linq;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Course> GetAllCourses() => FindAll(c => true);

        public IEnumerable<Course> GetCoursesByCategory(Guid catId)
        {
            var dbCtx = GetDbContext();
            var coursesFilterByCat = dbCtx.Set<Course>()
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .ToList();
            return coursesFilterByCat;
        }
    }
}
