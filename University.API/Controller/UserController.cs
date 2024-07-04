using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

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

    /// <summary>
    /// Default parameterized constructor.
    /// </summary>
    /// <param name="userContext"></param>
    public UserController(UserContext userContext) => _userRepository = new UserRepository(userContext);

    /// <summary>
    /// Method to get user by guid.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>An instance of <see cref="User"/> if exists, otherwise null.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _userRepository.GetUserById(id);

        return user == null ? NotFound() : Ok(user);
    }

    /// <summary>
    /// Method to create a new user.
    /// </summary>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    /// <returns></returns>
    [HttpPost]
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> UpdateUserPut(Guid id, User user)
    {
        
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> UpdateUserPatch(Guid id, User user)
    {
        
    }
    
    /// <summary>
    /// Method to delete a user.
    /// </summary>
    /// <param name="id">User's guid</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await  _userRepository.DeleteUser(id);
        return Ok();
    }
}