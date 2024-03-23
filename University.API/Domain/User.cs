namespace University.Domain;

public class User
{
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }

    public User() { }
    
    public User(string? username, string? email, string? passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}