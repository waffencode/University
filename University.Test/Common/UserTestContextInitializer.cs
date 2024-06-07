using Microsoft.EntityFrameworkCore;
using University.Infrastructure;
using University.Repository;

namespace University.Test.Common;

public class UserTestContextInitializer
{
    private readonly UserContext _context;

    public UserRepository UserRepository => new(_context);

    public UserTestContextInitializer()
    {
        DbContextOptionsBuilder<UserContext> builder = new();
        builder.UseInMemoryDatabase("TestDb");

        var dbContextOptions = builder.Options;
        _context = new UserContext(dbContextOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.ChangeTracker.Clear();
        _context.SaveChanges();
    }
}