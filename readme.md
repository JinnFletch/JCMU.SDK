# Jinn Context Menu Utility (JCMU) SDK

The JCMU SDK is a high-performance, Railway-Oriented framework for building dynamic Windows Context Menu extensions using .NET 8. 

Instead of grappling with C++ COM extensions or brittle Registry hacks, JCMU provides a modern, sandboxed environment where you can write standard C# logic and have it integrated into the Windows Shell instantly.

## 📦 Installation

1. Create a **.NET 8.0 Class Library**.
2. Add the NuGet package:
   ```shell
   dotnet add package JinnDev.JCMU.SDK
   ```
3. Create a `manifest.json` in your project root. Set its properties to **Copy to Output Directory: Copy if newer**.

---

## 1. The Manifest (`manifest.json`)

The manifest is the "ID Card" of your addon. It tells JCMU who you are and exactly how your menu should look.

```json
{
  "AddonId": "JCMU.GitUtils",
  "DisplayName": "Git Utility Pack",
  "Version": "1.1.0",
  "Author": "JinnDev",
  "MinCoreVersion": "1.0.4",
  "Menu": {
    "MenuItemName": "Initialize Repository",
    "IconPath": "C:\\Path\\To\\icon.ico",
    "Ordinal": 10,
    "Category": "Git Tools",
    "RunInBackground": false,
    "SubItems": null
  }
}
```

### Key Properties
| Property | Description |
| :--- | :--- |
| **`AddonId`** | A globally unique string. Used for isolated storage and folder naming. |
| **`Category`** | Optional. Groups your addon into a sub-folder (e.g., "Media Tools"). |
| **`Ordinal`** | Lower numbers appear higher in the list. |
| **`RunInBackground`** | If `true`, the addon runs via `jcmu-bg.exe` with no console window. |
| **`SubItems`** | Allows you to create infinitely nested cascading menus. |

---

## 2. The Code (`IJcmuAddon`)

Your addon must contain a public class implementing `IJcmuAddon`. JCMU uses specialized `AssemblyLoadContexts` to load your DLL and its dependencies in total isolation.

### The `ExecuteAsync` Contract
The return type is `Task<Maybe<int>>`. The integer controls the **Terminal UI behavior**:

*   **`return Maybe.Some(-1);`** : Keep the console open until the user presses a key.
*   **`return Maybe.Some(5);`**  : Success! Start a 5-second countdown then auto-close.
*   **`return Maybe.Some(0);`**  : Success! Close the window immediately.
*   **`return Maybe.None("Error");`**: Failure. Displays the error and starts a 10-second countdown.

### Implementation Example
```csharp
public class GitInitAction : IJcmuAddon
{
    public async Task<Maybe<int>> ExecuteAsync(ActionContext context)
    {
        var logger = context.HostServices.Logger;
        
        logger.LogInfo($"Initializing Git in: {context.TargetDirectory}");

        // Perform logic...
        
        return Maybe.Some(5); // Success! Close in 5 seconds.
    }
}
```

---

## 3. Host Services (`IHostServices`)

The `ActionContext` provides a bridge to the JCMU Core.

### 🛡️ Secure Settings & Secrets (`context.HostServices.Settings`)
JCMU provides a built-in, isolated storage engine for every addon.

*   **Standard Config:** Use `GetValueAsync<T>` and `SetValueAsync<T>`. Data is stored as plain-text JSON in `%LOCALAPPDATA%`.
*   **Secure Secrets:** Use `GetSecretAsync` and `SetSecretAsync`. Data is encrypted using **Windows DPAPI**. It can *only* be decrypted by the specific Windows User who saved it.

> [!IMPORTANT]
> To prevent secrets (like API Keys) from accidentally leaking into your plain-text config file, always mark your Secret properties with `[JsonIgnore]` in your model classes.

### 📝 Unified Logging (`context.HostServices.Logger`)
Every log call is automatically routed to two places:
1.  **The Console:** If `RunInBackground` is false.
2.  **The Core Log File:** A persistent rolling log located at `%PROGRAMDATA%\JCMU\Logs\`.

### ⌨️ User Input (`context.HostServices.PromptUserAsync`)
A thread-safe way to request input from the user (e.g., "Enter Commit Message:"). If the user cancels or closes the window, it returns a `Maybe.None`.

---

## 4. Local Development Workflow

JCMU is designed for rapid iteration. You do not need to "install" your addon while coding it.

1.  **Link your project:** Point JCMU to your build folder.
    ```shell
    jcmu dev link "C:\Source\MyAddonProject"
    ```
2.  **The Magic Junction:** JCMU creates a Windows Directory Junction from your `bin/Debug` folder directly into the Core engine.
3.  **Instant Updates:** Make a code change in Visual Studio and hit **Build**. Right-click your folder again—the changes are live instantly. No re-linking required.
4.  **Unlink:**
    ```shell
    jcmu dev unlink JCMU.MyAddon
    ```

---

## 5. Publishing

To join the decentralized JCMU ecosystem:
1.  Push your code to a public GitHub repository.
2.  Add the topic tag **`jcmu-addon`** to your repository.
3.  JCMU's global `search` command will now discover your addon automatically.

---

## 🛠️ SDK Architecture Note
The SDK utilizes **Railway-Oriented Programming (ROP)** via the `JinnDev.Utilities.Monad` library. We highly recommend following the monadic "Some/None" track pattern to ensure your context menu extensions are robust and never crash the user's Explorer process.