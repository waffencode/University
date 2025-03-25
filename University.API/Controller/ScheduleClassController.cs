using Microsoft.AspNetCore.Mvc;
using University.Domain;
using University.Infrastructure;
using University.Repository;

namespace University.Controller;

public class ScheduleClassController: CrudBaseController<ScheduleClass>
{
    public ScheduleClassController(UserContext context, ILogger<ClassTimeSlotController> logger) : base(context, logger)
    {
        _repository = new ScheduleClassRepository(context);
    }

    public override async Task<ActionResult<ScheduleClass>> Update(Guid id, ScheduleClass entity)
    {
        await _repository.UpdateAsync(entity);
        return Ok();
    }
}