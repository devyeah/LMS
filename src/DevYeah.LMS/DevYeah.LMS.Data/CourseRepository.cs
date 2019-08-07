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

        public IEnumerable<Course> GetAllCourses() => FindAll(course => true);

        public IEnumerable<Course> GetCoursesByCategory(Guid catId)
        {
            if (catId == Guid.Empty) return GetAllCourses();
            var dbCtx = GetDbContext();
            var coursesFilterByCat = dbCtx.Set<Course>()
                .Include(crs => crs.CourseCategory)
                    .ThenInclude(cc => cc.Category.Id == catId)
                .AsNoTracking()
                .ToList();
            return coursesFilterByCat;
        }
    }
}
