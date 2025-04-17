namespace University.Domain;

public class ScheduleClassMapper
{
    public static ScheduleClassDto ScheduleClassToScheduleClassDto(ScheduleClass scheduleClass)
    {
        return new ScheduleClassDto
        {
            Id = scheduleClass.Id,
            ClassroomId = scheduleClass.Classroom.Id,
            ClassType = scheduleClass.ClassType,
            Date = scheduleClass.Date,
            GroupsId = scheduleClass.Groups.Select(s => s.Id),
            Name = scheduleClass.Name,
            SubjectWorkProgramId = scheduleClass.SubjectWorkProgram.Id,
            TeacherId = scheduleClass.Teacher.Id,
            TimeSlotId = scheduleClass.TimeSlot.Id
        };
    }
}