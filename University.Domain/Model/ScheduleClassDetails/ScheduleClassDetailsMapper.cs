namespace University.Domain.Model;

public class ScheduleClassDetailsMapper
{
    public static ScheduleClassDetailsDto ScheduleClassDetailsToScheduleClassDetailsDto(ScheduleClassDetails scheduleClassDetails)
    {
        return new ScheduleClassDetailsDto
        {
            Id = scheduleClassDetails.Id,
            StudentDetailsDtoList = scheduleClassDetails.StudentDetailsList.Select(studentDetails => new StudentDetailsDto
            {
                Id = studentDetails.Id,
                StudentId = studentDetails.Student.Id,
                Attendance = studentDetails.Attendance,
                Grade = studentDetails.Grade
            }).ToList()
        };
    }
}