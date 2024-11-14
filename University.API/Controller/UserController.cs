using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;
using University.Service;

namespace University.Controller;

/// <summary>
/// API controller for managing users.
/// </summary>
/// <author>waffencode@gmail.com</author>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly IUserService _userService;
    
    /// <summary>
    /// Default parameterized constructor.
    /// </summary>
    /// <param name="userContext"></param>
    /// <param name="userService"></param>
    public UserController(UserContext userContext, IUserService userService)
    {
        _userRepository = new UserRepository(userContext);
        _userService = userService;
    }

    /// <summary>
    /// Method to get user by guid.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>An instance of <see cref="User"/> if exists, otherwise null.</returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _userRepository.GetUserById(id);

        return user == null ? NotFound() : Ok(user);
    }
    
    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();

        return users.Count == 0? NoContent() : Ok(users);
    }

    /// <summary>
    /// Method to create a new user.
    /// </summary>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        await _userRepository.CreateUser(user);
        var createdUserId = user.Id;
        
        var createdUser  = await  _userRepository.GetUserById(createdUserId);
        if (createdUser == null)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Method to update a user completely.
    /// </summary>
    /// <param name="id">Route parameter, Guid of user.</param>
    /// <param name="user">An instance of <see cref="User"/> to update.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> UpdateUserPut(Guid id, User user)
    {
        if (!id.Equals(user.Id))
        {
            return BadRequest();
        }

        try
        {
            await _userRepository.UpdateUserFully(id, user);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Method to update a user partially.
    /// </summary>
    /// <param name="id">Route parameter, Guid of user.</param>
    /// <param name="user">An instance of <see cref="User"/> to update.</param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> UpdateUserPatch(Guid id, User user)
    {
        if (!id.Equals(user.Id))
        {
            return BadRequest();
        }

        try
        {
            await _userRepository.UpdateUserPartially(id, user);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    /// <summary>
    /// Method to delete a user.
    /// </summary>
    /// <param name="id">User's guid</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _userRepository.DeleteUser(id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> Login(string email, string passwordHash)
    {
        try
        {
            var token = await _userService.Login(email, passwordHash);
            HttpContext.Response.Cookies.Append("token", token);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(string email, string passwordHash)
    {
        await _userService.Register(email, passwordHash);
        return Ok();
    }
    
    [HttpPost("authorize")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AuthorizeUser(string requestStringId)
    {
        var requestId = Guid.Parse(requestStringId);
        
        if (requestId.Equals( Guid.Empty))
        {
            return BadRequest();
        }
        
        await _userService.AuthorizeUser(requestId);
        return Ok();
    }
    
    // For testing purposes.
    [HttpGet("whoami")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> Whoami()
    {
        var rawId = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(rawId))
        {
            return NotFound("ID not found");
        }

        var id = Guid.Parse(rawId);
        
        var email = (await _userRepository.GetUserById(id))?.Email;
        
        return Ok(email);
    }
}