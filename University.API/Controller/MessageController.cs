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
    
    public MessageController(UserContext userContext)
    {
        _messageRepository = new MessageRepository(userContext);
        _userRepository = new UserRepository(userContext);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage(Message message)
    {
        await _messageRepository.AddMessage(message);
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
        await _messageRepository.DeleteMessage(id);
        return Ok();
    }
}