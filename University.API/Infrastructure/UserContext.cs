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
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Parameterized constructor.
    /// </summary>
    /// <param name="options">DbContextOptions for DbContext.</param>
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}