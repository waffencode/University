using University.Domain;
using System.ComponentModel.DataAnnotations;

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
        var id = Guid.NewGuid();
        var user = new User(id, "User", "example@example.com", "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b");
        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
        Assert.Equal("User", user.Username);
        Assert.Equal("example@example.com", user.Email);
        Assert.Equal("6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b", user.PasswordHash);
    }

    [Fact]
    public void UsernameFieldShouldBeChangeable()
    {
        var user = new User
        {
            Username = "Test"
        };
        Assert.Equal("Test", user.Username);
    }

    [Fact]
    public void EmailFieldShouldBeChangeable()
    {
        var user = new User
        {
            Email = "example@example.com"
        };
        Assert.Equal("example@example.com", user.Email);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("user@domain.com", true)]
    public void ShouldNotAcceptInvalidEmail(string email, bool isValid)
    {
        var user = new User
        {
            Email = email
        };
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(user);
        var result = Validator.TryValidateObject(user, validationContext, validationResults, true);
        Assert.Equal(isValid, result);
    }

    [Fact]
    public void PasswordHashFieldShouldBeChangeable()
    {
        var user = new User
        {
            PasswordHash = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b"
        };
        Assert.Equal("6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b", user.PasswordHash);
    }

    [Fact]
    public void IdFieldShouldBeNonEmptyGuid()
    {
        var user = new User();
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public void ShouldInitializeIdOnObjectInstantiation()
    {
        var id = Guid.NewGuid();
        var user = new User() { Id = id };
        Assert.Equal(id, user.Id);
    }
}