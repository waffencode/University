using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Test.Infrastructure;

public class UserContextTest
{
    private readonly DbContextOptions<UserContext> _options = new DbContextOptionsBuilder<UserContext>().Options;
    
    [Fact]
    public void ShouldInstantiate()
    {
        var context = new UserContext(_options);
        Assert.NotNull(context);
    }
    
    [Fact]
    public void ShouldBeDerivedFromDbContext()
    {
        var context = new UserContext(_options);
        Assert.IsAssignableFrom<DbContext>(context);
    }

    [Fact]
    public void ShouldContainDbSet()
    {
        var context  = new UserContext(_options);
        Assert.IsAssignableFrom<DbSet<User>>(context.Users);
        Assert.NotNull(context.Users);
    }
}