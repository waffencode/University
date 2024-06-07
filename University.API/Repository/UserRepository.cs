using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

/// <summary>
/// Repository for <see cref="User"/>. Implements CRUD operations.
/// </summary>
/// <author>waffencode@gmail.com</author>
public class UserRepository
{
    /// <summary>
    /// An instance of <see cref="UserContext"/>.
    /// </summary>
    public UserContext Context { get; }

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

    /// <summary>
    /// Async method that finds user by guid.
    /// </summary>
    /// <param name="id">Guid of user.</param>
    /// <returns>An instance of <see cref="User"/> if exists, otherwise null.</returns>
    public async Task<User?> GetUserById(Guid id) => await Context.Users.FirstOrDefaultAsync(x => x.Id == id);
}