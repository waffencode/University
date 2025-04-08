using Microsoft.EntityFrameworkCore;
using University.Domain;

namespace University.Infrastructure;

/// <summary>
/// User database context.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class UserContext : DbContext
{
    /// <summary>
    /// DbSet for <see cref="User"/>.
    /// </summary>
    public DbSet<User> Users { get; init; }
    
    public DbSet<RegistrationRequest> RegistrationRequests { get; init; }
    
    public DbSet<Message> Messages { get; init; }
    
    public DbSet<Classroom> Classrooms { get; init; }
    
    public DbSet<StudyGroup> StudyGroups { get; init; }
    
    public DbSet<ClassTimeSlot> ClassTimeSlots { get; init; }
    
    public DbSet<ScheduleClass> ScheduleClasses { get; init; }
    
    public DbSet<Subject> Subjects { get; init; }
    
    public DbSet<SubjectWorkProgram> SubjectWorkPrograms { get; init; }
    
    public DbSet<FieldOfStudy> FieldsOfStudy { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany<RegistrationRequest>()
            .WithOne(p => p.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasMany<Message>()
            .WithMany(p => p.Receivers);
        
        modelBuilder.Entity<User>()
            .HasMany<Message>()
            .WithOne(p => p.Sender);

        modelBuilder.Entity<SubjectWorkProgram>()
            .OwnsMany(p => p.Classes);

        modelBuilder.Entity<StudyGroup>()
            .HasOne(p => p.FieldOfStudy);
    }
    
    /// <summary>
    /// Parameterized constructor.
    /// </summary>
    /// <param name="options">DbContextOptions for DbContext.</param>
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}