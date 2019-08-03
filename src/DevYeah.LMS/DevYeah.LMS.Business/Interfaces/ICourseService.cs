using System;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    interface ICourseService
    {
        ServiceResult<CourseServiceResultCode> CreateCourse(SaveOrUpdateCourseRequest request);
        ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request);
        ServiceResult<CourseServiceResultCode> DeleteCourse(Guid courseId);
        ServiceResult<CourseServiceResultCode> GetCourseByKey(Guid courseId);
        ServiceResult<CourseServiceResultCode> AddCategory(string name);
        ServiceResult<CourseServiceResultCode> DeleteCategory(Guid categoryId);
        ServiceResult<CourseServiceResultCode> UpdateCategory(Guid categoryId, string name);
        ServiceResult<CourseServiceResultCode> GetCategoryByKey(Guid key);
        ServiceResult<CourseServiceResultCode> GetAllCategories();
    }
}
