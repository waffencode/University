using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using University.Domain;
using University.Infrastructure;
using University.Repository;
using University.Security;
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
    private readonly RegistrationRequestRepository _registrationRequestRepository;
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly JwtOptions _options;
    
    /// <summary>
    /// Default parameterized constructor.
    /// </summary>
    public UserController(UniversityContext universityContext, IUserService userService, ILogger<UserController> logger, IOptions<JwtOptions> options)
    {
        _userRepository = new UserRepository(universityContext);
        _registrationRequestRepository = new RegistrationRequestRepository(universityContext);
        _userService = userService;
        _logger = logger;
        _options = options.Value;
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

        return user is null ? NotFound() : Ok(user);
    }
    
    [HttpGet]
    [Authorize]
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
        
        var createdUser = await _userRepository.GetUserById(createdUserId);
        if (createdUser is null)
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
            _logger.LogInformation("User <{id}> successfully updated their info.", id);
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
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            var issuer = await _userRepository.GetUserById(GetRequestAuthorUserId()) ?? throw new InvalidOperationException("Issuer not found.");
            var userToDelete = await _userRepository.GetUserById(id) ?? throw new InvalidOperationException();

            if (userToDelete.Equals(issuer))
            {
                return BadRequest();
            }
            
            await _userRepository.DeleteUser(id);
            
            _logger.LogInformation("User {deleted_id} <{email}> was deleted. Issued by {issuer_id} <{issuer_email}>", 
                userToDelete.Id, 
                userToDelete.Email, 
                issuer.Id,
                issuer.Email);
            
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Expires = DateTime.Now.AddHours(_options.ExpireHours)
            };

            var token = await _userService.Login(email, passwordHash);
            if (!token.Equals(string.Empty))
            {
                HttpContext.Response.Cookies.Append("token", token, cookieOptions);
                _logger.LogInformation("User <{email}> successfully logged in.", email);
            }
            else
            {
                _logger.LogInformation("Failed to log in <{email}>: null token is passed (hash={hash}).", email, token.GetHashCode());
                throw new Exception("Authentication token is null or empty.");
            }
            
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        _logger.LogInformation("User <{email}> logged out.", 
            User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("userId")?.Value);
        HttpContext.Response.Cookies.Delete("token");
        return Ok();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(User user)
    {
        await _userService.Register(user);
        _logger.LogInformation("User <{email}> successfully registered. Created registration request {requestId}.", 
            user.Email,
            (await _userService.GetUserPendingRegistrationRequestAsync(user.Id)).Id);
        return Ok();
    }
    
    [HttpGet("authorize/{requestId:guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AuthorizeUser(Guid requestId)
    {
        try
        {
            await _userService.AuthorizeUser(requestId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("pendingRequests")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<RegistrationRequest>>> GetPendingRegistrationRequests()
    {
        var requests = await _registrationRequestRepository.GetPendingRegistrationRequests();
        return requests.Count == 0 ? NoContent() : Ok(requests);
    }

    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> CheckCookie()
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience  = false,
            ValidateLifetime  = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
        };
        
        var authCookie = HttpContext.Request.Cookies["token"];
        if (authCookie is null)
        {
            return Ok(false);
        }
       
        try
        {
            var jwt = await new JwtSecurityTokenHandler().ValidateTokenAsync(authCookie, validationParameters);
            return Ok(jwt is not null);
        }
        catch (SecurityTokenException ex)
        {
            return Ok(false);
        }
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserProfile()
    {
        var userId = GetRequestAuthorUserId();
        return Ok(await _userRepository.GetUserById(userId));
    }

    [NonAction]
    private Guid GetRequestAuthorUserId()
    {
        var rawId = User.FindFirst("userId")?.Value;

        return string.IsNullOrEmpty(rawId)
            ? throw new InvalidOperationException("Failed to parse GUID of the request issuer.")
            : Guid.Parse(rawId);
    }
}