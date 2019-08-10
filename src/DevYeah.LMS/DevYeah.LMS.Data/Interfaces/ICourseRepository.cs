using System;
using System.Collections.Generic;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetCoursesOfCategory(Guid catId);
    }
}
