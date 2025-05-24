using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;
using University.Service;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class MessageController(IMessageService service, IMessageRepository messageRepository, IUserRepository userRepository, ILogger<UserController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage(MessageDto message, CancellationToken cancellationToken)
    {
        try
        {
            await messageRepository.AddAsync(message, cancellationToken);
            logger.LogInformation("Message <{id}> has been sent", message.Id);
            return CreatedAtAction(nameof(SendMessage), message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Message>>> GetMessagesForUser(Guid id)
    {
        var user = await userRepository.GetUserById(id) ?? throw new Exception("User not found");
        return Ok(await messageRepository.GetMessagesByReceiver(user));
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        try
        {
            await messageRepository.DeleteMessage(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("receivers/{id:guid}")]
    [Authorize]
    public async Task<ActionResult<List<User>>> GetAvailableReceivers(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await service.GetAvailableReceiversForUserAsync(id, cancellationToken));
    }
}