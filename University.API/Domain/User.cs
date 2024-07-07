using System.ComponentModel.DataAnnotations;

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
    [StringLength(maximumLength: 64, MinimumLength = 3)]
    public string? Username { get; set; }
    
    /// <summary>
    /// User's hashed password.
    /// </summary>
    /// <remarks>Maximum length is defined as SHA-256 hash string length.</remarks>
    [StringLength(maximumLength: 64)]
    public string? PasswordHash { get; set; }
    
    /// <summary>
    /// User's email address.
    /// </summary>
    [StringLength(maximumLength: 254, MinimumLength = 3)]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    public UserRole Role { get; set; } = UserRole.Unauthorized;
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public User() { }
    
    /// <summary>
    /// Parameterized constructor.
    /// </summary>
    /// <param name="id">User's unique identifier.</param>
    /// <param name="username">User's login name.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="passwordHash">User's hashed password.</param>
    /// <param name="role">User's role.</param>
    public User(Guid id, string? username, string? email, string? passwordHash, UserRole role)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    /// <summary>
    /// Creates a new User object with properties selectively updated from a target User.
    /// </summary>
    /// <param name="target">The User object containing the new data to update.</param>
    /// <returns>A new User object with updated properties.</returns>
    /// <remarks>
    /// This method creates a new User instance with its properties set to the values from the target User.
    /// If a property in the target User is null, the corresponding property in the new User retains current value.
    /// </remarks>
    public User GetPartiallyUpdatedUser(User target)
    {
        var user = new User
        {
            Id = target.Id,
            Username = target.Username ?? Username,
            Email = target.Email ?? Email,
            PasswordHash = target.PasswordHash ?? PasswordHash,
            Role = target.Role
        };

        return user;
    }
}