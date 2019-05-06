using DevYeah.LMS.Models;
using System;
using System.Collections.Generic;

namespace DevYeah.LMS.Business.Interfaces
{
    interface IStudentService
    {
        Account FindStudentByKey(Guid accountId);
        void EnrollCourse(Guid accountId, Guid courseId);
        // enroll multiple courses one time
        void EnrollCourses(Guid accountId, List<Guid> courseIds);
        // get all courses of a specific student
        List<Course> GetAllCoursesByKey(Guid accountId);
        // return: id of UserProfile
        Guid? UpdateProfile(Guid accountId, UserProfile userProfile);
        void BeginTopic(Guid accountId, Guid topicId);
        void EndTopic(Guid accountId, Guid topicId);
        int GetCourseProgress(Guid accoutId, Guid courseId);
    }
}
