using System;
using System.Collections.Generic;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Business
{
    public class CourseService : ServiceBase, ICourseService
    {
        private static readonly string ArgumentErrorMsg = "Argument is not correct.";
        private static readonly string InternalErrorMsg = "Server internal error.";

        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository)
        {
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
        }
        public ServiceResult<CourseServiceResultCode> CreateCourse(SaveOrUpdateCourseRequest request)
        {
            var isValidRequest = ValidateCreateCourseRequest(request);
            if (!isValidRequest) throw new ArgumentException(ArgumentErrorMsg);

            try
            {
                var course = MakeNewCourse(request);
                _courseRepository.Add(course);
                _courseRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return BuildResult(false, CourseServiceResultCode.BackenException, InternalErrorMsg);
            }
            
        }

        private Course MakeNewCourse(SaveOrUpdateCourseRequest request)
        {
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Overview = request.Overview,
                InstructorId = request.InstructorId,
                Edition = request.Edition,
                Level = request.Level,
                AvgLearningTime = request.AvgLearningTime
            };
            var courseCategories = ClassifyCourseCategory(request.Categories, course);
            course.CourseCategory = courseCategories;
            return course;
        }

        private IList<CourseCategory> ClassifyCourseCategory(Guid[] categories, Course course)
        {
            var resultCats = new List<CourseCategory>();
            Category cat;
            foreach(var item in categories)
            {
                cat = _categoryRepository.Get(item);
                if(cat != null)
                    resultCats.Add(new CourseCategory {
                        Category = cat,
                        Course = course
                    });
            }
            return resultCats;
        }

        private bool ValidateCreateCourseRequest(SaveOrUpdateCourseRequest request)
        {
            if (request == null) return false;
            if (request.InstructorId == null) return false;
            if (string.IsNullOrWhiteSpace(request.Name)) return false;
            if (string.IsNullOrWhiteSpace(request.Overview)) return false;
            if (request.Edition <= 0 || request.Level <= 0 || request.AvgLearningTime <= 0) return false;

            return true;
        }

        public void DeleteCourse(string courseId)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<CourseServiceResultCode> GetAllTopicsOfCourse(string courseId)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<CourseServiceResultCode> GetCourse(string courseId)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
