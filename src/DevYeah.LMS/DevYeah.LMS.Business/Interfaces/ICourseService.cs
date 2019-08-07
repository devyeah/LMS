using System;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    public interface ICourseService
    {
        ServiceResult<CourseServiceResultCode> CreateCourse(SaveOrUpdateCourseRequest request);
        ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request);
        ServiceResult<CourseServiceResultCode> DeleteCourse(Guid courseId);
        ServiceResult<CourseServiceResultCode> GetCourseByKey(Guid courseId);
        ServiceResult<CourseServiceResultCode> GetAllCoursesOfCategory(Guid catId);
        ServiceResult<CourseServiceResultCode> GetAllCourses();
        ServiceResult<CourseServiceResultCode> AddCategory(AddOrUpdateCategoryRequest request);
        ServiceResult<CourseServiceResultCode> DeleteCategory(Guid categoryId);
        ServiceResult<CourseServiceResultCode> UpdateCategory(AddOrUpdateCategoryRequest request);
        ServiceResult<CourseServiceResultCode> GetCategoryByKey(Guid key);
        ServiceResult<CourseServiceResultCode> GetAllCategories();
    }
}
