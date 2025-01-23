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
    
    public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
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
        
        // TODO: Add StudyGroup relation when StudyGroup is implemented.
    }
    
    /// <summary>
    /// Parameterized constructor.
    /// </summary>
    /// <param name="options">DbContextOptions for DbContext.</param>
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}