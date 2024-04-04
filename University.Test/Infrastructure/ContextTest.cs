using Microsoft.EntityFrameworkCore;
using University.Infrastructure;

namespace University.Test.Infrastructure;

public class ContextTest
{
    [Fact]
    public void ShouldInstantiate()
    {
        var context = new Context();
        Assert.NotNull(context);
    }
    
    [Fact]
    public void ShouldBeDerivedFromDbContext()
    {
        var context = new Context();
        Assert.IsAssignableFrom<DbContext>(context);
    }
}