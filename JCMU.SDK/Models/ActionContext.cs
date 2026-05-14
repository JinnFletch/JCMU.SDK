using JinnDev.JCMU.SDK.Interfaces;

namespace JinnDev.JCMU.SDK.Models;

/// <summary>
/// Encapsulates the state and available toolset at the exact moment a user triggers the context menu.
/// </summary>
public record ActionContext
{
    /// <summary>
    /// The absolute path of the directory that the user right-clicked on.
    /// </summary>
    public required string TargetDirectory { get; init; }

    /// <summary>
    /// A suite of utilities (CLI runner, logger, prompts) provided by the Core engine 
    /// for the Addon to use during its execution.
    /// </summary>
    public required IHostServices HostServices { get; init; }
}