namespace University.Domain;

/// <summary>
/// Represents an individual user with account data.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class User
{
    public Guid Id { get; } = Guid.NewGuid();
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