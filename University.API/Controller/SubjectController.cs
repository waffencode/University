using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private SubjectRepository Repository { get; }
    private readonly ILogger<SubjectController> _logger;

    public SubjectController(UniversityContext context, ILogger<SubjectController> logger)
    {
        Repository = new SubjectRepository(context);
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Subject>>> GetAll()
    {
        return Ok(await Repository.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<Subject>> Get(Guid id)
    {
        return Ok(await Repository.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post(Subject subject)
    {
        await Repository.AddAsync(subject);
        return CreatedAtAction(nameof(Get), new {id = subject.Id}, subject);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Put(Guid id, Subject subject)
    {
        await Repository.UpdateAsync(subject);
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Repository.DeleteAsync(id);
        return Ok();
    }
}