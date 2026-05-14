using JinnDev.JCMU.SDK.Models;
using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Interfaces;

/// <summary>
/// The core contract that every JCMU plugin must implement. 
/// The JCMU Core engine will locate and instantiate the class implementing this interface.
/// </summary>
public interface IJcmuAddon
{
    /// <summary>
    /// Retrieves the metadata and identity of the addon.
    /// </summary>
    /// <returns>A monad containing the validated plugin manifest.</returns>
    Maybe<PluginManifest> GetManifest();

    /// <summary>
    /// Retrieves the static configuration defining how this addon integrates into the Windows Registry context menu.
    /// </summary>
    /// <returns>A monad containing the validated menu definition.</returns>
    Maybe<MenuDefinition> GetMenuRegistration();

    /// <summary>
    /// Executes the core logic of the addon when the user clicks the context menu item.
    /// </summary>
    /// <param name="context">The state and toolset provided by the host at the moment of execution.</param>
    /// <returns>A parameterless monad representing the success or failure of the execution.</returns>
    Task<Maybe> ExecuteAsync(ActionContext context);
}