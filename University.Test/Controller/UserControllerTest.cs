using System.Net;
using Microsoft.AspNetCore.Mvc;
using University.Controller;
using University.Domain;
using University.Repository;
using University.Test.Common;

namespace University.Test.Controller;

public class UserControllerTest
{
    private UserRepository TestRepository { get; }
    
    public UserControllerTest()
    {
        var initializer = new UserTestContextInitializer();
        TestRepository = initializer.UserRepository;
    }
    
    [Fact]
    public void ShouldInitialize()
    {
        var controller = new UserController(TestRepository.Context);
        Assert.NotNull(controller);
    }
    
    [Fact]
    public void ShouldBeDerivedFromControllerBase()
    {
        var controller = new UserController(TestRepository.Context);
        Assert.IsAssignableFrom<ControllerBase>(controller);
    }
    
    [Fact]
    public async Task ResponseShouldBeActionResult()
    {
        var controller = new UserController(TestRepository.Context);
        var result = await controller.GetUser(Guid.NewGuid());
        Assert.IsAssignableFrom<ActionResult<User>>(result);
    }
    
    [Fact]
    public async Task ShouldReturnUserIfExists()
    {
        var user = new User(Guid.NewGuid(), "User", "example@example.com",
            "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b");
        var controller = new UserController(TestRepository.Context);
        
        await TestRepository.CreateUser(user);
        var result = await controller.GetUser(user.Id);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(user, okResult.Value);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundCodeForNonExistingUser()
    {
        var controller = new UserController(TestRepository.Context);
        var result = await controller.GetUser(Guid.NewGuid());
        Assert.Equal(404, (result.Result as NotFoundResult)?.StatusCode);
    }
}