using Microsoft.AspNetCore.Mvc;
using University.Utility;

namespace University.Controller;

/// <summary>
/// Controller class which contains methods to check the version of the application.
/// </summary>
/// <author>waffencode@gmail.com</author>
[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{
    /// <summary>
    /// Method to get the version of the application.
    /// </summary>
    /// <response code="200">Returns the version of the application.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public ActionResult<string> GetVersion() => Ok(VersionInfo.AssemblyVersion);
}