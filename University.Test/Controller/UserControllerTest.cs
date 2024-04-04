using Microsoft.AspNetCore.Mvc;
using University.Controller;

namespace University.Test.Controller;

public class UserControllerTest
{
    [Fact]
    public void ShouldInitialize()
    {
        var controller = new UserController();
        Assert.NotNull(controller);
    }
    
    [Fact]
    public void ShouldBeDerivedFromControllerBase()
    {
        var controller = new UserController();
        Assert.IsAssignableFrom<ControllerBase>(controller);
    }
}