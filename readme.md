# Jinn Context Menu Utils SDK (JCMU.SDK)

The **JCMU.SDK** is the official, ultra-lightweight contract for building plugins (Addons) for the Jinn Context Menu Utils platform. 

By referencing this SDK, you can build powerful C# plugins that integrate directly into the Windows File Explorer right-click context menu, without needing to understand COM Shell Extensions or registry hacking.

## 🏗️ Philosophy
* **The "Handshake" Only:** This SDK defines *only* the communication layer between the JCMU Core and your plugin. It does not force heavy utility libraries (like CLI runners or JSON parsers) on you. You bring your own tools.
* **Decoupled UI:** Plugins do not render their own UI. You provide the logic, and the JCMU Core engine handles the rendering, progress updates, and user prompts (whether running in a Console, WPF, or background process).
* **Railway-Oriented (Monadic):** Every API surface uses the `JinnDev.Utilities.Monad` library to eliminate `try-catch` blocks and `null` reference exceptions.

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
Defines how your plugin appears in the Windows right-click menu. The Core handles all Windows Registry editing on your behalf.
```csharp
public Maybe<MenuDefinition> GetMenuRegistration()
{
    return new MenuDefinition
    {
        MenuItemName = "Do Awesome Thing",
        Ordinal = 10,
        Placement = MenuPlacement.FileSystem, // Groups it cleanly with similar tools
        RunInBackground = true // Logic executes silently without a console window
    };
}
```

### 3. Execution
When a user right-clicks a folder and selects your menu item, the Core engine invokes `ExecuteAsync`. You are provided an `ActionContext` which contains:
* `TargetDirectory`: The absolute path of the folder the user right-clicked.
* `HostServices`: A toolbelt provided by the Core to safely interact with the user interface.

```csharp
public async Task<Maybe> ExecuteAsync(ActionContext context)
{
    var logger = context.HostServices.Logger;

    logger.LogInfo($"Scanning directory: {context.TargetDirectory}");

    // Example: Using the Host's UI abstraction to ask a question
    var userInput = await context.HostServices.PromptUserAsync("What should we name the backup?");
    
    if (!userInput.HasValue)
        return Maybe.Fail("Operation cancelled by user.");

    // ... Do your actual logic here ...

    return Maybe.SUCCESS;
}
```

## 🛠️ The Host Services Toolbelt
To keep plugins headless and adaptable to any UI, the JCMU Core provides several utilities through `context.HostServices`:
* **`Logger`**: Standardized output (`LogInfo`, `LogWarning`, `LogError`). The Host decides how this is rendered (e.g., colored console text, toast notifications, or log files).
* **`PromptUserAsync(message)`**: Requests string input from the user safely, without assuming the presence of `Console.ReadLine()`.
* **`RunInBackground`**: Determines the visibility of the execution.
    * `false` (Default): A console window opens to show logs and allow user input. Use this for verbose tasks or interactive scripts.
    * `true`: The addon executes silently in the background. Use this for "transparent" actions like quick file deletions or background syncs.

*(Note: If your plugin needs to execute command-line instructions, we highly recommend adding the `JinnDev.Utilities.CommandLine` NuGet package to your addon project.)*

## 🛡️ Working with the Maybe Monad
Because the SDK enforces the `Maybe` Monad, you never return `void` or `null`.
* If your execution finishes successfully, return `Maybe.SUCCESS`.
* If you encounter an error (or a condition where the plugin shouldn't run), return `Maybe.Fail("Reason")`. The JCMU Core will automatically intercept this, log it appropriately, and handle the failure gracefully.