using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly MessageRepository _messageRepository;
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    
    public MessageController(UniversityContext universityContext, ILogger<UserController> logger)
    {
        _messageRepository = new MessageRepository(universityContext);
        _userRepository = new UserRepository(universityContext);
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage(Message message)
    {
        await _messageRepository.AddMessage(message);
        _logger.LogInformation("Message <{id}> has been sent", message.Id);
        return CreatedAtAction(nameof(SendMessage), message);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Message>>> GetMessagesForUser(Guid id)
    {
        var user = await _userRepository.GetUserById(id) ?? throw new Exception("User not found");
        return Ok(await _messageRepository.GetMessagesByReceiver(user));
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        try
        {
            await _messageRepository.DeleteMessage(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}