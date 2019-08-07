using System;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Business.RequestModels;
using DevYeah.LMS.Business.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace DevYeah.LMS.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost("addcat")]
        public IActionResult CreateNewCategory(AddOrUpdateCategoryRequest request) => GetResult(() => _courseService.AddCategory(request));

        [HttpGet("findallcats")]
        public IActionResult FetchAllCategories() => GetResult(() => _courseService.GetAllCategories());

        [HttpPost("addcourse")]
        public IActionResult CreateNewCourse(SaveOrUpdateCourseRequest request) => GetResult(() => _courseService.CreateCourse(request));

        [HttpGet("findcats/{catId}")]
        public IActionResult FetchCoursesByCategory(Guid catId) => GetResult(() => _courseService.GetAllCoursesOfCategory(catId));

        [HttpGet("findallcourses")]
        public IActionResult FetchAllCourses() => GetResult(() => _courseService.GetAllCourses());

        private IActionResult GetResult(Func<ServiceResult<CourseServiceResultCode>> action)
        {
            var result = action();
            if (result.IsSuccess) return Ok(result.ResultObj);

            return BadRequest(result.Message);
        }
    }
}
