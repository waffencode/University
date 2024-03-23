using University.Domain;

namespace University.Test.Domain;

public class UserTest
{
    [Fact]
    public void ShouldInstantiateUserWithDefaultConstructor()
    {
        var user = new User();
        Assert.NotNull(user);
    }

    [Fact]
    public void ShouldInstantiateUserWithConstructorWithParameters()
    {
        var user = new User("User", "example@example.com", "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b");
        Assert.NotNull(user);
        Assert.Equal("User", user.Username);
        Assert.Equal("example@example.com", user.Email);
        Assert.Equal("6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b", user.PasswordHash);
    }
}