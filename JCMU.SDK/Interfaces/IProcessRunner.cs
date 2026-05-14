using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Interfaces;

/// <summary>
/// A safe abstraction for executing command-line instructions within the host environment.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Executes a single command in the specified working directory.
    /// </summary>
    /// <param name="workingDirectory">The absolute path where the command should be run.</param>
    /// <param name="command">The CLI command to execute (e.g., "git status").</param>
    /// <returns>A monad containing the standard output lines if successful, or the error output/exception if it fails.</returns>
    Task<Maybe<IReadOnlyList<string>>> RunCommandAsync(string workingDirectory, string command);

    /// <summary>
    /// Executes a sequence of commands in the specified working directory, maintaining state between them.
    /// </summary>
    /// <param name="workingDirectory">The absolute path where the commands should be run.</param>
    /// <param name="commands">The list of CLI commands to execute sequentially.</param>
    /// <returns>A monad containing a dictionary mapping the command index to its output lines.</returns>
    Task<Maybe<IReadOnlyDictionary<int, IReadOnlyList<string>>>> RunCommandsAsync(string workingDirectory, IEnumerable<string> commands);
}
