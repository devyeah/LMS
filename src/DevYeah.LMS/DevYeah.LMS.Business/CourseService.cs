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
        private static readonly string RequestFailureMsg = "Your request failed.";

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
            if (!isValidRequest) return BuildResult(false, CourseServiceResultCode.ArgumentError, ArgumentErrorMsg);

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
            if (string.IsNullOrWhiteSpace(request.Overview)) return false;
            if (request.Edition <= 0 || request.Level <= 0 || request.AvgLearningTime <= 0) return false;
            if (request.InstructorId == null) return false;
            if (string.IsNullOrWhiteSpace(request.Name)) return false;

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

        public ServiceResult<CourseServiceResultCode> AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BuildResult(false, CourseServiceResultCode.ArgumentError, ArgumentErrorMsg);

            var category = new Category { Id = Guid.NewGuid(), Name = name };
            try
            {
                _categoryRepository.Add(category);
                _categoryRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return BuildResult(false, CourseServiceResultCode.BackenException, InternalErrorMsg);
            }
        }

        public ServiceResult<CourseServiceResultCode> DeleteCategory(Guid categoryId)
        {
            if (categoryId == null || categoryId.Equals(Guid.Empty))
                return BuildResult(false, CourseServiceResultCode.ArgumentError, ArgumentErrorMsg);

            try
            {
                var category = _categoryRepository.Get(categoryId);
                if (category == null) return BuildResult(false, CourseServiceResultCode.DataNotExist, RequestFailureMsg);
                _categoryRepository.Delete(category);
                _categoryRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return BuildResult(false, CourseServiceResultCode.BackenException, InternalErrorMsg);
            }
        }

        public ServiceResult<CourseServiceResultCode> UpdateCategory(Guid categoryId, string name)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<CourseServiceResultCode> GetCategory(Guid key)
        {
            if (key == null || key.Equals(Guid.Empty))
                return BuildResult(false, CourseServiceResultCode.ArgumentError, ArgumentErrorMsg);

            try
            {
                var category = _categoryRepository.Get(key);
                if (category == null) return BuildResult(false, CourseServiceResultCode.DataNotExist, RequestFailureMsg);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception)
            {
                return BuildResult(false, CourseServiceResultCode.BackenException, InternalErrorMsg);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetAllCategories()
        {
            try
            {
                var allCategories = _categoryRepository.FindAll(cat => true);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: allCategories);
            }
            catch (Exception)
            {
                return BuildResult(false, CourseServiceResultCode.BackenException, InternalErrorMsg);

            }

        }
    }
}
