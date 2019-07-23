using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    interface ICourseService
    {
        ServiceResult<CourseServiceResultCode> CreateCourse(SaveOrUpdateCourseRequest request);
        ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request);
        void DeleteCourse(string courseId);
        ServiceResult<CourseServiceResultCode> GetCourse(string courseId);
        ServiceResult<CourseServiceResultCode> GetAllTopicsOfCourse(string courseId);
    }
}
