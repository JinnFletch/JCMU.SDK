using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Interfaces;

/// <summary>
/// A centralized toolbelt provided by the JCMU Core to the Addon during execution.
/// </summary>
public interface IHostServices
{
    /// <summary>
    /// Gets the logger instance for the addon to report progress and errors.
    /// </summary>
    IPluginLogger Logger { get; }

    /// <summary>
    /// Gets the command-line execution runner.
    /// </summary>
    IProcessRunner CLI { get; }

    /// <summary>
    /// Prompts the user for input in the host's interface (e.g., the Console).
    /// </summary>
    /// <param name="message">The question or prompt to display to the user.</param>
    /// <returns>A monad containing the user's input, or a failure state if the prompt was cancelled/failed.</returns>
    Task<Maybe<string>> PromptUserAsync(string message);
}
