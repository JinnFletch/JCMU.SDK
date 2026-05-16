# Jinn Context Menu Utility (JCMU) SDK

The JCMU SDK provides the core contracts required to build dynamic, C#-powered Windows Context Menu extensions. 

Instead of messing with the Windows Registry or building complex COM servers, JCMU allows you to write standard C# logic, define a simple JSON manifest, and let the JCMU Core handle the OS integration, memory isolation, and execution.

## Getting Started

1. Create a new C# Class Library targeting `.NET 8.0`.
2. Add the `JinnDev.JCMU.SDK` NuGet package.
3. Add a `manifest.json` file to your project root.
4. Implement the `IJcmuAddon` interface.

---

## 1. The Manifest (`manifest.json`)

The manifest defines your addon's identity and exactly how it should appear in the Windows Right-Click menu. 

Create a `manifest.json` in your project and ensure its **Build Action** is set to `Content` and **Copy to Output Directory** is set to `Copy if newer`.

```json
{
  "AddonId": "JCMU.MyAwesomeAddon",
  "DisplayName": "My Awesome Addon",
  "Version": "1.0.0",
  "Author": "YourName",
  "MinCoreVersion": "1.0.0",
  "Menu": {
    "MenuItemName": "Do Something Awesome",
    "Ordinal": 10,
    "Category": "My Tools",
    "RunInBackground": false,
    "SubItems": null
  }
}
```

### Menu Properties
* **`MenuItemName`**: The text the user actually clicks.
* **`Ordinal`**: Determines the sorting order. Lower numbers appear higher in the list.
* **`Category`**: (Optional) Groups your addon into a specific sub-folder in the root menu.
* **`RunInBackground`**: If `true`, your addon will execute silently without popping up a console window.
* **`SubItems`**: (Optional) An array of nested menu items if you want to create a cascading menu tree.

---

## 2. The Code (`IJcmuAddon`)

The JCMU Core engine will automatically discover and instantiate any public class in your compiled `.dll` that implements `IJcmuAddon`. 

You only need to implement a single method: `ExecuteAsync`.

```csharp
using JinnDev.JCMU.SDK.Interfaces;
using JinnDev.JCMU.SDK.Models;
using JinnDev.Utilities.Monad;

namespace MyAwesomeAddon;

public class AwesomeAction : IJcmuAddon
{
    public async Task<Maybe> ExecuteAsync(ActionContext context)
    {
        var targetDir = context.TargetDirectory;
        var logger = context.HostServices.Logger;

        logger.LogInfo($"Executing addon against folder: {targetDir}");

        try
        {
            // Your custom C# logic goes here
            // e.g., File.Create(Path.Combine(targetDir, "new_file.txt"));
            
            return Maybe.SUCCESS;
        }
        catch (Exception ex)
        {
            logger.LogError("Something went wrong!", ex);
            return Maybe.Fail(ex.Message);
        }
    }
}
```

---

## 3. The Execution Context

When a user right-clicks a folder and selects your addon, your `ExecuteAsync` method is provided an `ActionContext`. This contains everything you need to interact with the environment safely.

### `TargetDirectory`
A string representing the absolute path of the folder the user right-clicked on.

### `HostServices`
A toolbelt provided by the JCMU Core bridging your isolated addon to the host OS.
* **`Logger`**: Use `LogInfo()`, `LogWarning()`, and `LogError()` to safely write output. If your menu is set to `RunInBackground: false`, this prints to the user's console window.
* **`PromptUserAsync(string message)`**: A safe way to ask the user for text input (e.g., asking for a commit message or a new file name). 

---

## 4. Publishing Your Addon

JCMU features a decentralized package manager powered by GitHub. 

To make your addon available to the world:
1. Push your source code to a public GitHub repository.
2. Add the topic tag **`jcmu-addon`** to your repository on GitHub.
3. Users can now discover your addon by typing `jcmu search` in their console!