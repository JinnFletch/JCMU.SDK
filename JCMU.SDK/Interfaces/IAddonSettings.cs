using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Interfaces;

/// <summary>
/// Provides isolated configuration and secure secrets management for a JCMU Addon.
/// Values are stored locally in the current user's profile.
/// </summary>
public interface IAddonSettings
{
    /// <summary>
    /// Retrieves a standard configuration value.
    /// </summary>
    /// <typeparam name="T">The expected type of the configuration value.</typeparam>
    /// <param name="key">The unique key for the setting.</param>
    /// <returns>A monad containing the value if found, or a failure state if missing/unparseable.</returns>
    Task<Maybe<T>> GetValueAsync<T>(string key);

    /// <summary>
    /// Saves a standard configuration value.
    /// </summary>
    /// <typeparam name="T">The type of the value being saved.</typeparam>
    /// <param name="key">The unique key for the setting.</param>
    /// <param name="value">The value to save.</param>
    /// <returns>A parameterless monad representing success or failure of the disk write.</returns>
    Task<Maybe> SetValueAsync<T>(string key, T value);

    /// <summary>
    /// Retrieves and decrypts a secure secret (e.g., API key, authentication token).
    /// </summary>
    /// <param name="key">The unique key for the secret.</param>
    /// <returns>A monad containing the decrypted string if found, or a failure state if missing/corrupted.</returns>
    Task<Maybe<string>> GetSecretAsync(string key);

    /// <summary>
    /// Encrypts and saves a secure secret using Windows DPAPI. 
    /// The secret can only be decrypted by the Windows User Account that encrypted it.
    /// </summary>
    /// <param name="key">The unique key for the secret.</param>
    /// <param name="value">The plaintext string to encrypt and save.</param>
    /// <returns>A parameterless monad representing success or failure of the encryption and disk write.</returns>
    Task<Maybe> SetSecretAsync(string key, string value);
}