using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

/// <summary>
/// Repository for <see cref="User"/>. Implements CRUD operations.
/// </summary>
/// <param name="context">An instance of the <see cref="UniversityContext"/>.</param>
/// <author>waffencode@gmail.com</author>
public class UserRepository(UniversityContext context) : IUserRepository
{
    /// <summary>
    /// Async method that adds user to the database.
    /// </summary>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    public async Task CreateUser(User user)
    {
        if (await IsUserExist(user.Id))
        {
            throw new InvalidOperationException($"A user with the ID {user.Id} already exists in the database.");
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously returns all users.
    /// </summary>
    /// <returns></returns>
    public async Task<List<User>> GetAllUsers() => await context.Users.ToListAsync();

    /// <summary>
    /// Async method that finds user by guid.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>An instance of <see cref="User"/> if exists, otherwise null.</returns>
    public async Task<User?> GetUserById(Guid id) => await context.Users.FirstOrDefaultAsync(x => x.Id == id);

    /// <summary>
    /// Asynchronously returns a list of users with specified ids.
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<User>> GetUsersByIds(List<Guid> ids, CancellationToken cancellationToken) =>
        await context.Users.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <summary>
    /// Async method that ensures that user exists.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>true if user exists, otherwise false</returns>
    public async Task<bool> IsUserExist(Guid id) => await context.Users.AnyAsync(u => u.Id == id);

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

        context.Users.Remove(user);
        await context.SaveChangesAsync();
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

        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronous method to update user partially. New value will replace existing one only if new value is not null.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <param name="user">An instance of <see cref="User"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if user not found.</exception>
    // TODO: Fix error.
    public async Task UpdateUserPartially(Guid id, User user)
    {
        var currentUser = await GetUserById(id);
        if (currentUser == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var updatedUser = currentUser.GetPartiallyUpdatedUser(user);
        context.Users.Update(updatedUser);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a User object from the database by email address.
    /// </summary>
    /// <param name="email">The email address to search for in the User records.</param>
    /// <returns>A Task resulting in the User object with the specified email.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no User with the given email is found.</exception>
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return user;
    }

    public IQueryable<User> GetAllAsIQueryable() =>
        context.Users.AsNoTracking().AsSplitQuery().AsQueryable();

    public async Task<List<User>> GetUnauthorisedUsers() =>
        await context.Users.Where(x => x.Role == UserRole.Unauthorized).ToListAsync();

    /// <summary>
    /// Asynchronously finds a user which name contains the specified substring.
    /// </summary>
    /// <param name="nameSubstring"></param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="User"/> if found, otherwise null.</returns>
    public async Task<IEnumerable<User>> FindUserByName(string nameSubstring, CancellationToken cancellationToken) =>
        await context.Users.Where(x => x.FullName.Contains(nameSubstring)).ToListAsync(cancellationToken);
}