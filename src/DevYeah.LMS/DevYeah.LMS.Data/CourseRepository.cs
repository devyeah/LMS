using System;
using System.Collections.Generic;
using System.Linq;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using DevYeah.LMS.Data.Pagination;
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
            var query = GetQueryExpOfCourse();
            return query.ToList();
        }

        public PagedResult<Course> GetPaginatedCourses(int page, int pageSize)
        {
            var query = GetQueryExpOfCourse();
            return query.GetPaged(page, pageSize);
        }

        public IEnumerable<Course> GetCoursesOfCategory(Guid catId)
        {
            var query = GetQueryExpOfCourse();
            return query
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .ToList();
        }

        public PagedResult<Course> GetPaginatedCoursesOfCategory(Guid catId, int page, int pageSize)
        {
            var query = GetQueryExpOfCourse();
            return query
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .GetPaged(page, pageSize);
        }

        private IQueryable<Course> GetQueryExpOfCourse()
        {
            var dbCtx = GetDbContext();
            return dbCtx.Set<Course>()
                .Include(c => c.Instructor)
                .AsNoTracking();
        }
    }
}
