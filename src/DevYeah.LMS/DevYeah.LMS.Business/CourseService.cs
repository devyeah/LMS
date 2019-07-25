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
            if (!isValidRequest) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = MakeNewCourse(request);
                _courseRepository.Add(course);
                _courseRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
            
        }

        public ServiceResult<CourseServiceResultCode> DeleteCourse(Guid courseId)
        {
            if (courseId == null || courseId.Equals(Guid.Empty))
                return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = _courseRepository.Get(courseId);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                _courseRepository.Delete(course);
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetCourseByKey(Guid courseId)
        {
            if (courseId == null || courseId.Equals(Guid.Empty)) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = _courseRepository.Get(courseId);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: course);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request)
        {
            var isValidRequest = ValidateUpdateCourseRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);            

            try
            {
                var course = _courseRepository.Get(request.Id);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                UpdateDataOfCourse(course, request);
                _courseRepository.Update(course);
                _courseRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: course);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            var category = new Category { Id = Guid.NewGuid(), Name = name };
            try
            {
                _categoryRepository.Add(category);
                _categoryRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> DeleteCategory(Guid categoryId)
        {
            if (categoryId == null || categoryId.Equals(Guid.Empty))
                return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepository.Get(categoryId);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                _categoryRepository.Delete(category);
                _categoryRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> UpdateCategory(Guid categoryId, string name)
        {
            if (categoryId == null || categoryId.Equals(Guid.Empty) || string.IsNullOrWhiteSpace(name))
                return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepository.Get(categoryId);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                if (name == category.Name) return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
                category.Name = name;
                _categoryRepository.Update(category);
                _categoryRepository.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetCategoryByKey(Guid key)
        {
            if (key == null || key.Equals(Guid.Empty)) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepository.Get(key);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception)
            {
                return InternalErrorResult(CourseServiceResultCode.BackenException);
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
                return InternalErrorResult(CourseServiceResultCode.BackenException);

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
            foreach (var item in categories)
            {
                cat = _categoryRepository.Get(item);
                if (cat != null)
                    resultCats.Add(new CourseCategory
                    {
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

        private void UpdateDataOfCourse(Course course, SaveOrUpdateCourseRequest request)
        {
            course.InstructorId = request.InstructorId;
            course.Level = request.Level;
            course.Overview = request.Overview;
            course.Name = request.Name;
            course.AvgLearningTime = request.AvgLearningTime;
            course.CourseCategory = ClassifyCourseCategory(request.Categories, course);
        }

        private bool ValidateUpdateCourseRequest(SaveOrUpdateCourseRequest request)
        {
            var isValidContent = ValidateCreateCourseRequest(request);
            var isValidKey = !(request.Id == null || request.Id.Equals(Guid.Empty));
            return (isValidContent && isValidKey);
        }
    }
}
