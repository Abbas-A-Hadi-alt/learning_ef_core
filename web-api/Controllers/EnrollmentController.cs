using business_layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.DTOs.Students;
using Modules.Entities;

namespace web_api.Controllers;

[Authorize]
[ApiController]
[Route("api/Enrollments")]
public class EnrollmentController(EnrollmentService enrollmentServices) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("EachStudentWithItNumberOfRegisteredCourses", Name = "GetEachStudentWithNumberOfRegisteredCourses")]
    public async Task<ActionResult<List<StudentCourseCountDto>>> GetEachStudentWithNumberOfRegisteredCourses()
    {
        var result = await enrollmentServices.GetEachStudentWithNumberOfRegisteredCourses();

        if (result.Count is 0)
        {
            return NotFound("No students found with registered courses.");
        }

        return Ok(result);
    }
}
