using System;
using System.Collections.Generic;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;

namespace DevYeah.LMS.Business.Interfaces
{
    interface IStudentService
    {
        ServiceResult<StudentServiceResultCode> GetStudent(Guid accountId);
        void EnrollCourse(Guid accountId, Guid courseId);
        // enroll multiple courses one time
        void EnrollCourses(Guid accountId, IList<Guid> courseIds);
        // get all courses of a specific student
        ServiceResult<StudentServiceResultCode> GetAllCourses(Guid accountId);
        // return: id of UserProfile
        ServiceResult<StudentServiceResultCode> UpdateProfile(UpdateUserProfileRequest request);
        void BeginTopic(Guid accountId, Guid topicId);
        void EndTopic(Guid accountId, Guid topicId);
        int GetCourseProgress(Guid accoutId, Guid courseId);
    }
}
