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
            var dbCtx = GetDbContext();
            var coursesFilterByCat = dbCtx.Set<Category>()
                .Include(t => t.CourseCategory)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(c => c.Instructor)
                .Where(t => t.Id == catId)
                .AsNoTracking()
                .FirstOrDefault();
            return coursesFilterByCat.CourseCategory.Select(t => new Course{
                Id = t.Course.Id,
                Name = t.Course.Name,
                InstructorId = t.Course.Instructor.Id,
                Overview = t.Course.Overview,
                Edition = t.Course.Edition,
                Level = t.Course.Level,
                AvgLearningTime = t.Course.AvgLearningTime
            });
        }
    }
}
