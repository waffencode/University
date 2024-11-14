namespace University.Domain;

public class RegistrationRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public User User { get; init; }
    public UserRole RequestedRole { get; init; }
    public UserRole CurrentRole { get; init; }
    public DateTime Date { get; init; }
    public RegistrationRequestStatus Status { get; set; }

    public RegistrationRequest()
    {
        if (User is null)
        {
            throw new ArgumentNullException(nameof(User));
        }

        if (RequestedRole is UserRole.Unauthorized)
        {
            throw new ArgumentNullException(nameof(RequestedRole));
        }
        
        Status = RegistrationRequestStatus.Pending;
        CurrentRole = User.Role;
        Date = DateTime.Now;
    }
}