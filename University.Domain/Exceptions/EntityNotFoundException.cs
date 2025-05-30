namespace University.Exceptions;

/// <summary>
/// Represents an exception which is thrown when an entity is not found in the database.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class EntityNotFoundException: Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> with a specific <see cref="Type"/> and an ID.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the entity which was not found.</param>
    /// <param name="id">The ID of the entity which was not found.</param>
    /// <example>This code: <code>throw new EntityNotFoundException(typeof(User), Guid.Empty.ToString())</code>
    /// will throw this exception with the following message:
    /// <code>User with the ID 00000000-0000-0000-0000-000000000000 was not found in the database.</code>
    /// </example>
    public EntityNotFoundException(Type type, string id)
        : base($"The {type.Name} with the ID {id} was not found in the database.")
    {
    }
}