using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using University.Controller;
using University.Utility;

namespace University.Test.Controller;

public class VersionControllerTest
{
    [Fact]
    public void ShouldInitialize()
    {
        var controller = new VersionController();
        Assert.NotNull(controller);
    }

    [Fact]
    public void ShouldBeDerivedFromControllerBase()
    {
        var controller = new VersionController();
        Assert.IsAssignableFrom<ControllerBase>(controller);
    }
    
    [Fact]
    public void ResponseShouldBeActionResult()
    {
        var controller = new VersionController();
        var result = controller.GetVersion();
        Assert.IsAssignableFrom<ActionResult<string>>(result);
    }
    
    [Fact]
    public void ShouldReturnOkCode()
    {
        var controller = new VersionController();
        var result = controller.GetVersion();
        Assert.IsType<OkObjectResult>(result.Result);
    }
    
    [Fact]
    public void ResultShouldNotBeNull()
    {
        var controller = new VersionController();
        var result = controller.GetVersion();
        var versionResult = (result.Result as OkObjectResult)?.Value as string;
        Assert.NotNull(versionResult);
    }
    
    [Fact]
    public void ResultShouldContainValidVersion()
    {
        var controller = new VersionController();
        var result = controller.GetVersion();
        var versionResult = (result.Result as OkObjectResult)?.Value as string;
        Assert.Equal(VersionInfo.AssemblyVersion, versionResult);
    }
}