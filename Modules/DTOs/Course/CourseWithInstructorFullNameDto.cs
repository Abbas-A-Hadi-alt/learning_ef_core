namespace Modules.DTOs.Course;

public record CourseWithInstructorFullNameDto
{
    public string Code { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string InstructorFirstName { get; init; } = null!;
    public string InstructorLastName { get; init; } = null!;
};