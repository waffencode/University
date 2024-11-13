using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

/// <summary>
/// Repository for <see cref="User"/>. Implements CRUD operations.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// An instance of <see cref="UserContext"/>.
    /// </summary>
    private UserContext Context { get; }

    /// <summary>
    /// Default parameterized constructor. Parameter should not be null.
    /// </summary>
    /// <param name="context">An instance of <see cref="UserContext"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if parameter is null.</exception>
    public UserRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    /// <summary>
    /// Async method that adds user to the database.
    /// </summary>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    public async Task CreateUser(User user)
    {
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsers() => await Context.Users.ToListAsync();

    /// <summary>
    /// Async method that finds user by guid.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>An instance of <see cref="User"/> if exists, otherwise null.</returns>
    public async Task<User?> GetUserById(Guid id) => await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

    /// <summary>
    /// Async method that ensures that user exists.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>true if user exists, otherwise false</returns>
    public async Task<bool> IsUserExist(Guid id)  => await Context.Users.AnyAsync(u => u.Id == id);
    
    /// <summary>
    /// Async method to delete user.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <exception cref="InvalidOperationException">Thrown if user not found.</exception>
    public async Task DeleteUser(Guid id)
    {
        var user = await GetUserById(id);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }
        
        Context.Users.Remove(user);
        await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Async method to update user fully. If user's field value is null, it will replace existing value.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if user not found.</exception>
    public async Task UpdateUserFully(Guid id, User user)
    {
        if (!await IsUserExist(id))
        {
            throw new InvalidOperationException("User not found");
        }
        
        Context.Users.Update(user);
        await Context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Asynchronous method to update user partially. New value will replace existing one only if new value is not null.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if user not found.</exception>
    public async Task UpdateUserPartially(Guid id, User user)
    {
        var currentUser = await GetUserById(id);
        if (currentUser == null)
        {
            throw new InvalidOperationException("User not found");
        }
        
        var updatedUser = currentUser.GetPartiallyUpdatedUser(user);
        Context.Users.Update(updatedUser);
        await Context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Asynchronously retrieves a User object from the database by email address.
    /// </summary>
    /// <param name="email">The email address to search for in the User records.</param>
    /// <returns>A Task resulting in the User object with the specified email.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no User with the given email is found.</exception>
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }
        
        return user;
    }
}