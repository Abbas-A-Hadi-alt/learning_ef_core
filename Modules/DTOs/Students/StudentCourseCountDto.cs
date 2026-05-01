namespace Modules.DTOs.Students;
public record StudentCourseCountDto
{
    public int StudentId { get; init; }
    public string StudentFirstName { get; init; } = null!;
    public string StudentLastName { get; init; } = null!;
    public int NumberOfRegisteredCourses { get; init; }
};