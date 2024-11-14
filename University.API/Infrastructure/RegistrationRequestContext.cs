using Microsoft.EntityFrameworkCore;
using University.Domain;

namespace University.Infrastructure;

public class RegistrationRequestContext : DbContext
{
    public DbSet<RegistrationRequest> RegistrationRequests { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany<RegistrationRequest>()
            .WithOne(p => p.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public RegistrationRequestContext(DbContextOptions<RegistrationRequestContext> options) : base(options) { }
}