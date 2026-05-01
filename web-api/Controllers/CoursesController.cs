using business_layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.DTOs.Course;

namespace web_api.Controllers;

[Authorize]
[ApiController]
[Route("api/Courses")]
public class CoursesController(CourseService courseService) : Controller
{
    [AllowAnonymous]
    [HttpGet("EachCourseWithInstructorFullName", Name = "GetEachCourseWithInstructorFullName")]
    public async Task<ActionResult<List<CourseWithInstructorFullNameDto>>> GetEachCourseWithInstructorFullName()
    {
        var result = await courseService.GetEachCourseWithInstructorFullName();
        
        if (result.Count is 0)
        {
            return NotFound("No courses found.");
        }

        return Ok(result);
    }
}
