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
        private readonly ICourseRepository _courseRepo;
        private readonly ICategoryRepository _categoryRepo;

        public CourseService(ICourseRepository courseRepo, ICategoryRepository categoryRepo, ISystemErrorsRepository systemErrorsRepo) 
            : base(systemErrorsRepo)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
        }

        public ServiceResult<CourseServiceResultCode> CreateCourse(SaveOrUpdateCourseRequest request)
        {
            var isValidRequest = ValidateCreateCourseRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = MakeNewCourse(request);
                _courseRepo.Add(course);
                _courseRepo.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
            
        }

        public ServiceResult<CourseServiceResultCode> DeleteCourse(Guid courseId)
        {
            if (courseId == Guid.Empty)
                return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = _courseRepo.Get(courseId);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                _courseRepo.Delete(course);
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetCourseByKey(Guid courseId)
        {
            if (courseId == Guid.Empty) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var course = _courseRepo.Get(courseId);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: course);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetAllCourses()
        {
            try
            {
                var allCourses = _courseRepo.GetAllCourses();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: allCourses);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetAllCoursesOfCategory(Guid catId)
        {
            if (catId == Guid.Empty) return GetAllCourses();
            try
            {
                var isValidCat = _categoryRepo.IsExisted(catId);
                if (!isValidCat) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                var courses = _courseRepo.GetCoursesOfCategory(catId);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: courses);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> UpdateCourse(SaveOrUpdateCourseRequest request)
        {
            var isValidRequest = ValidateUpdateCourseRequest(request);
            if (!isValidRequest) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);            

            try
            {
                var course = _courseRepo.Get(request.Id);
                if (course == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                UpdateDataOfCourse(course, request);
                _courseRepo.Update(course);
                _courseRepo.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: course);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> AddCategory(AddOrUpdateCategoryRequest request)
        {
            var isValidArgs = ValidateAddCategoryRequest(request);
            if (!isValidArgs) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            var category = new Category { Id = Guid.NewGuid(), Name = request.Name, Icon = request.Icon };
            try
            {
                if (_categoryRepo.IsExistedName(request.Name)) return DataErrorResult(CourseServiceResultCode.DataDuplicated);
                _categoryRepo.Add(category);
                _categoryRepo.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> DeleteCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepo.Get(categoryId);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                _categoryRepo.Delete(category);
                _categoryRepo.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> UpdateCategory(AddOrUpdateCategoryRequest request)
        {
            var isValidArgs = ValidateUpdateCategoryRequest(request);
            if (!isValidArgs) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepo.Get(request.Id);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                if (request.Name == category.Name && request.Icon == request.Icon)
                    return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
                category.Name = request.Name;
                category.Icon = request.Icon;
                _categoryRepo.Update(category);
                _categoryRepo.SaveChanges();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetCategoryByKey(Guid key)
        {
            if (key == Guid.Empty) return ArgumentErrorResult(CourseServiceResultCode.ArgumentError);

            try
            {
                var category = _categoryRepo.Get(key);
                if (category == null) return DataErrorResult(CourseServiceResultCode.DataNotExist);
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: category);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
            }
        }

        public ServiceResult<CourseServiceResultCode> GetAllCategories()
        {
            try
            {
                var allCategories = _categoryRepo.FindAllCategories();
                return BuildResult(true, CourseServiceResultCode.Success, resultObj: allCategories);
            }
            catch (Exception ex)
            {
                _systemErrorsRepo.AddLog(ex);
                return InternalErrorResult(CourseServiceResultCode.BackendException);
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
                cat = _categoryRepo.Get(item);
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
            var isValidKey = !(request.Id == Guid.Empty);
            return (isValidContent && isValidKey);
        }

        private bool ValidateAddCategoryRequest(AddOrUpdateCategoryRequest request)
        {
            if (request == null) return false;
            if (string.IsNullOrWhiteSpace(request.Name)) return false;
            if (string.IsNullOrWhiteSpace(request.Icon)) return false;

            return true;
        }

        private bool ValidateUpdateCategoryRequest(AddOrUpdateCategoryRequest request)
        {
            var isValidData = ValidateAddCategoryRequest(request);
            var isValidKey = request.Id == Guid.Empty;
            return (isValidData && isValidKey);
        }
    }
}
