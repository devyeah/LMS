using System;
using System.Collections.Generic;
using DevYeah.LMS.Data.Pagination;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetCoursesOfCategory(Guid catId);
        PagedResult<Course> GetPaginatedCourses(int page, int pageSize);
        PagedResult<Course> GetPaginatedCoursesOfCategory(Guid catId, int page, int pageSize);
    }
}
