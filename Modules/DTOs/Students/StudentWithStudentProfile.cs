using Modules.Entities;

namespace Modules.DTOs.Students;

public record StudentWithStudentProfile
{
    public Student Student { get; init; } = null!;
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public string? Bio { get; init; }
    public string? LinkedInUrl { get; init; }
}