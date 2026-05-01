namespace Modules.Entities;

public partial class StudentProfile
{
    public int StudentId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }

    public virtual Student Student { get; set; } = null!;
}

public partial class StudentProfile
{
    public static StudentProfile Empty = new StudentProfile()
    {
        StudentId = -1,
        Address = string.Empty,
        City = string.Empty,
        Country = string.Empty,
        Bio = string.Empty,
        LinkedInUrl = string.Empty,
    };

    public static StudentProfile StudentWithEmptyProfile(Student student)
        => (Empty.Student = student)
                .StudentProfile!;
}