using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Repository;

namespace University.Controller;

[Route("api/[controller]")]
[ApiController]
public class StudyGroupController(IStudyGroupRepository repository, ILogger<StudyGroupController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudyGroupDto>>> Get()
    {
        var studyGroups = await repository.GetAllAsync();
        return Ok(studyGroups);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudyGroupDto>> Get(Guid id)
    {
        return Ok(await repository.GetByIdAsync(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<StudyGroup>> CreateStudyGroup(StudyGroupDto studyGroup, CancellationToken cancellationToken)
    {
        await repository.AddAsync(studyGroup, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = studyGroup.Id }, studyGroup);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StudyGroup>> Put(Guid id, StudyGroupDto studyGroup)
    {
        if (id != studyGroup.Id) return BadRequest();

        await repository.UpdateAsync(studyGroup);
        return Ok(studyGroup);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await repository.DeleteAsync(id);
        return Ok();
    }
}