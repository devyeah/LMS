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
    }
}
