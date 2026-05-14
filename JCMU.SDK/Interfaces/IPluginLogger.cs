using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Interfaces;

/// <summary>
/// Provides standardized logging capabilities for Addons, routing output back to the Core's UI or log files.
/// </summary>
public interface IPluginLogger
{
    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <returns>A parameterless success monad.</returns>
    Maybe LogInfo(string message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message.</param>
    /// <returns>A parameterless success monad.</returns>
    Maybe LogWarning(string message);

    /// <summary>
    /// Logs an error message, optionally including an exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="ex">The optional exception that caused the error.</param>
    /// <returns>A parameterless success monad.</returns>
    Maybe LogError(string message, Exception? ex = null);
}
