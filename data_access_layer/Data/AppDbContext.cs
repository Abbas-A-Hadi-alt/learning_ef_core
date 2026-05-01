using Modules.Entities;
using Microsoft.EntityFrameworkCore;

namespace data_access_layer.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Enrollment> Enrollments { get; set; }
    public virtual DbSet<Instructor> Instructors { get; set; }
    public virtual DbSet<StudentProfile> StudentProfiles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Course entity
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.InstructorId, "IX_Courses_InstructorId");
            
            entity.HasIndex(e => e.Status, "IX_Courses_Status");

            entity.HasIndex(e => e.Code, "UQ_Courses_Code")
                .IsUnique();

            entity.Property(e => e.Code).HasMaxLength(30);
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(500);

            // Begginer | Intermediate | Advanced
            entity.Property(e => e.Level).HasMaxLength(30);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)");

            // Active | Inactive | Archived
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.Property(e => e.Title).HasMaxLength(150);

            // Relationships:
            entity.HasOne(c => c.Instructor)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_Instructors");
        });


        // Configure Enrollment entity
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");
            entity.HasIndex(e => e.StudentId, "IX_Enrollments_StudentId");

            entity.HasIndex(e => e.Status, "IX_Enrollments_Status");

            // Prevent duplicate enrollments for the same student and course
            entity.HasIndex(e => new { e.StudentId, e.CourseId }, 
                                "UQ_Enrollments_StudentId_CourseId")
                .IsUnique();

            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime");

            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.ProgressPercent)
                .HasColumnType("decimal(5, 2)");

            entity.Property(e => e.FinalGrade)
                .HasColumnType("decimal(5, 2)");

            entity.Property(e => e.Status).HasMaxLength(20);


            // Relationships:
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_Enrollments_Courses");

            entity.HasOne(e => e.Student)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .HasConstraintName("FK_Enrollments_Students");
        });

        // Configure Instructor entity
        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasIndex(e => e.ManagerId, "IX_Instructors_ManagerId");

            entity.HasIndex(e => e.Email, "UQ_Instructors_Email")
                .IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(30);

            entity.Property(e => e.LastName).HasMaxLength(30);

            entity.Property(e => e.Email).HasMaxLength(150);

            entity.Property(e => e.HireDate)
                .HasColumnType("date");

            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Relationships:
            entity.HasOne(i => i.Manager)
                .WithMany(i => i.InverseManager)
                .HasForeignKey(i => i.ManagerId)
                .HasConstraintName("FK_Instructors_Manager");
        });

        // Configure Student entity
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.Status, "IX_Students_Status");

            entity.HasIndex(e => e.Email, "UQ_Students_Email")
                .IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.Property(e => e.Email).HasMaxLength(150);

            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date");

            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Status).HasMaxLength(20);

            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
        });

        // Configure StudentProfile entity
        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(e => e.StudentId);
         
            entity.Property(e => e.StudentId)
                .ValueGeneratedNever();

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Bio).HasMaxLength(500);
            entity.Property(e => e.LinkedInUrl).HasMaxLength(200);

            // Relationships:
            entity.HasOne(sp => sp.Student)
                .WithOne(s => s.StudentProfile)
                .HasForeignKey<StudentProfile>(sp => sp.StudentId)
                .HasConstraintName("FK_StudentProfiles_Students");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
