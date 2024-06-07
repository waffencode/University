using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;
using University.Repository;
using University.Test.Common;

namespace University.Test.Repository;

public class UserRepositoryTest
{
    private UserRepository TestRepository { get; }
    
    public UserRepositoryTest()
    {
        var initializer = new UserTestContextInitializer();
        TestRepository = initializer.UserRepository;
    }
    
    [Fact]
    public void ShouldInitialize()
    {
        DbContextOptionsBuilder<UserContext> builder = new();
        builder.UseInMemoryDatabase("TestDb");
        var testContext  = new UserContext(builder.Options);
        
        var repository = new UserRepository(testContext);
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task ShouldCreateAndGetUserEntity()
    {
        var user = new User(Guid.NewGuid(), "User", "example@example.com",
            "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b");
        var repository = new UserRepository(TestRepository.Context);
        
        await repository.CreateUser(user);
        var userEntity = await repository.GetUserById(user.Id);
        
        Assert.Equal(user, userEntity);
    }

    [Fact]
    public async Task ShouldReturnNullForNonExistingUser()
    {
        var repository = new UserRepository(TestRepository.Context);
        
        var userEntity = await repository.GetUserById(Guid.Empty);
        
        Assert.Null(userEntity);
    }
}