namespace University.Utility;

/// <summary>
/// Represents the version information of the application.
/// </summary>
/// <author>waffencode@gmail.com</author>
public static class VersionInfo
{
    /// <summary>
    /// Contains the version of the assembly.
    /// </summary>
    /// <remarks>Scenario where AssemblyVersion is null can be considered impossible.</remarks>
    public static string AssemblyVersion { get; } =
        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
}