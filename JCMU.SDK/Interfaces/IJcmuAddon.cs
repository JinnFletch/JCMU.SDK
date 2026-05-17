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
    /// Executes the core logic of the addon when the user clicks the context menu item.
    /// </summary>
    /// <param name="context">The state and toolset provided by the host at the moment of execution.</param>
    /// <returns>A monad where NONE is failure, 0 or more is a pause before closing time, and -1 is press any key to continue.</returns>
    Task<Maybe<int>> ExecuteAsync(ActionContext context);
}