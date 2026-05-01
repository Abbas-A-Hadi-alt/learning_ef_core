using business_layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Entities;

namespace web_api.Controllers;

[Authorize]
[ApiController]
[Route("api/Students")]
public class StudentsController(StudentService studentService) : ControllerBase
{

    [AllowAnonymous]
    [HttpGet("All", Name = "GetAllStudents")]
    public async Task<ActionResult<List<Student>>> GetAllStudents()
    {
        var students = await studentService.GetAllStudents();

        if (students.Count is 0)
        {
            return NotFound("No Student Found");
        }

        return Ok(students);
    }


    [AllowAnonymous]
    [HttpGet("AllByStatus/{status}", Name = "GetAllStudentsByStatus")]
    public async Task<ActionResult<List<Student>>> GetAllStudentsByStatus(string status)
    {
        var students = await studentService.GetAllStudentsByStatus(status);

        if (students.Count is 0)
        {
            return NotFound($"No {status} Student Found");
        }

        return Ok(students);
    }


    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetStudentById")]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var student = await studentService.GetStudentById(id);

        if (student is null)
        {
            return NotFound($"No Student Found with ID {id}");
        }

        return Ok(student);
    }
}