namespace University.Exceptions;

public class EntityNotFoundException: Exception
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }
    
    public EntityNotFoundException(Type type, string id) 
        : base($"{type.Name} with the ID {id} was not found in the database.")
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}