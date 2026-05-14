# Jinn Context Menu Utils SDK (JCMU.SDK)

The **JCMU.SDK** is the official, zero-dependency contract for building plugins (Addons) for the Jinn Context Menu Utils platform. 

By referencing this SDK, you can build powerful, dynamic C# plugins that integrate directly into the Windows File Explorer right-click context menu, without needing to understand COM Shell Extensions or registry hacking.

## 🏗️ Philosophy
* **Zero Dependencies:** The SDK is extremely lightweight. To prevent "DLL Hell", it relies only on native C# and the `JinnDev.Utilities.Monad` library.
* **Decoupled UI:** Plugins do not render their own UI. You provide the logic, and the JCMU Core engine handles the CLI rendering, progress bars, and user prompts.
* **Railway-Oriented (Monadic):** Every API surface uses the `Maybe` Monad to eliminate `try-catch` blocks and `null` reference exceptions.

## 🚀 Getting Started

1. Create a new **Class Library** project (.NET 8+).
2. Install the SDK via NuGet:
   ```bash
   dotnet add package JinnDev.JCMU.SDK
   ```
3. Create a public class that implements `IJcmuAddon`.

## 📖 The Core Contract

Every plugin must implement the `IJcmuAddon` interface. The JCMU Core engine will automatically discover and load your class.

### 1. The Manifest
Defines the identity and metadata of your plugin.
```csharp
public Maybe<PluginManifest> GetManifest()
{
    return new PluginManifest
    {
        AddonId = "MyOrg.AwesomePlugin",
        DisplayName = "Awesome JCMU Plugin",
        Version = "1.0.0",
        Author = "Your Name"
    };
}
```

### 2. The Menu Registration
Defines how your plugin appears in the Windows right-click menu.
```csharp
public Maybe<MenuDefinition> GetMenuRegistration()
{
    return new MenuDefinition
    {
        MenuItemName = "Do Awesome Thing",
        Ordinal = 10,
        Placement = MenuPlacement.Root // Or place it inside sub-menus like GitTools
    };
}
```

### 3. Execution
When a user right-clicks a folder and selects your menu item, the Core engine invokes `ExecuteAsync`. You are provided an `ActionContext` which contains:
* `TargetDirectory`: The absolute path of the folder the user right-clicked.
* `HostServices`: A toolbelt provided by the Core to log messages, prompt the user, or run CLI commands.

```csharp
public async Task<Maybe> ExecuteAsync(ActionContext context)
{
    var logger = context.HostServices.Logger;
    var cli = context.HostServices.CLI;

    logger.LogInfo($"Executing in: {context.TargetDirectory}");

    // Use the built-in process runner to safely execute command line tools
    var result = await cli.RunCommandAsync(context.TargetDirectory, "git status");

    if (!result.HasValue)
    {
        return Maybe.Fail("This folder is not a Git repository.");
    }

    return Maybe.SUCCESS;
}
```

## 🛠️ The Host Services Toolbelt
To keep plugins lightweight, the JCMU Core provides several utilities through `context.HostServices`:
* **`Logger`**: Standardized output (`LogInfo`, `LogWarning`, `LogError`).
* **`CLI`**: A safe `IProcessRunner` that executes shell commands and returns the output lines wrapped in a Monad, automatically catching native Win32 errors.
* **`PromptUserAsync(message)`**: Requests string input from the user via the Core's UI.

## 🛡️ Working with the Maybe Monad
Because the SDK enforces the `Maybe` Monad, you never return `void` or `null`.
* If your execution finishes successfully, return `Maybe.SUCCESS`.
* If you encounter an error (or a condition where the plugin shouldn't run), return `Maybe.Fail("Reason")`. The JCMU Core will automatically intercept this, log it in red, and keep the console open so the user can read the error.