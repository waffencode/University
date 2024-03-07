using System.Reflection;
using University.Utility;

namespace University.Test.Version;

public class VersionInfoTest
{
    [Fact]
    public void AssemblyVersionShouldNotBeNull()
    {
        Assert.NotNull(VersionInfo.AssemblyVersion);
    }

    [Fact]
    public void ShouldReturnCurrentAssemblyVersion()
    {
        var version = Assembly.GetAssembly(typeof(VersionInfo))!.GetName().Version?.ToString();
        Assert.Equal(version, VersionInfo.AssemblyVersion);
    }
}