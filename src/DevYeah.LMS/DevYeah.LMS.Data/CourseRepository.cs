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

        public IEnumerable<Course> GetAllCourses()
        {
            var dbCtx = GetDbContext();
            var allCourses = dbCtx.Set<Course>()
                .Include(c => c.Instructor)
                .ToList();
            return allCourses;
        }

        public IEnumerable<Course> GetCoursesByCategory(Guid catId)
        {
            var dbCtx = GetDbContext();
            var filteredCourses = dbCtx.Set<Course>()
                .Include(c => c.Instructor)
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .ToList();
            return filteredCourses;
        }
    }
}
