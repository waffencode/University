using Microsoft.EntityFrameworkCore;
using University.Domain;

namespace University.Infrastructure;

public class RegistrationRequestContext : DbContext
{
    public DbSet<RegistrationRequest> RegistrationRequests { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegistrationRequest>()
            .HasOne(p => p.User)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
    
    public RegistrationRequestContext(DbContextOptions<RegistrationRequestContext> options) : base(options) { }
}