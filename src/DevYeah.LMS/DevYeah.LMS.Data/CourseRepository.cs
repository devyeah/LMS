using System;
using System.Collections.Generic;
using System.Linq;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using DevYeah.LMS.Data.paginate;
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
            var allCourses = query.ToList();
            return allCourses;
        }

        public PagedResult<Course> GetPaginatedCourses(int page, int pageSize)
        {
            var query = GetQueryExpOfCourse();
            var paginatedCourses = query.GetPaged(page, pageSize);
            return paginatedCourses;
        }

        public IEnumerable<Course> GetCoursesOfCategory(Guid catId)
        {
            var query = GetQueryExpOfCourse();
            var filteredCourses = query
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .ToList();
            return filteredCourses;
        }

        public PagedResult<Course> GetPaginatedCoursesOfCategory(Guid catId, int page, int pageSize)
        {
            var query = GetQueryExpOfCourse();
            var filteredCourses = query
                .Where(c => c.CourseCategory.Any(t => t.CategoryId == catId))
                .GetPaged(page, pageSize);
            return filteredCourses;
        }

        private IQueryable<Course> GetQueryExpOfCourse()
        {
            var dbCtx = GetDbContext();
            var query = dbCtx.Set<Course>()
                .Include(c => c.Instructor);
            return query;
        }
    }
}
