namespace University.Domain;

/// <summary>
/// Represents an individual user with account data.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class User
{
    /// <summary>
    /// User's unique identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    /// <summary>
    /// User's login name.
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// User's hashed password.
    /// </summary>
    public string? PasswordHash { get; set; }
    /// <summary>
    /// User's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public User() { }
    
    /// <summary>
    /// Parameterized constructor.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    public User(Guid id, string? username, string? email, string? passwordHash)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}