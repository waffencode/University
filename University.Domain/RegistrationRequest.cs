namespace University.Domain;

public class RegistrationRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required User User { get; init; }
    public UserRole RequestedRole { get; init; }
    public UserRole CurrentRole { get; init; } = UserRole.Unauthorized;
    public DateTime Date { get; init; } = DateTime.Now;
    public RegistrationRequestStatus Status { get; set; } = RegistrationRequestStatus.Pending;
}