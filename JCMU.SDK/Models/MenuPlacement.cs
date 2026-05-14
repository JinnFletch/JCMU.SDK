namespace JinnDev.JCMU.SDK.Models;

/// <summary>
/// Defines where the Addon's context menu should be placed within the Windows Registry hierarchy.
/// </summary>
public enum MenuPlacement
{
    /// <summary>
    /// Places the addon directly under the main "Jinn Utils" context menu.
    /// </summary>
    Root,

    /// <summary>
    /// Groups the addon inside a predefined "Git Tools" sub-menu.
    /// </summary>
    GitTools,

    /// <summary>
    /// Groups the addon inside a predefined "File System" sub-menu.
    /// </summary>
    FileSystem,

    /// <summary>
    /// Groups the addon inside a predefined "Code Generation" sub-menu.
    /// </summary>
    CodeGeneration
}
