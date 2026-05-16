using JinnDev.Utilities.Monad;

namespace JinnDev.JCMU.SDK.Models;

/// <summary>
/// Defines the static configuration for how the addon should be registered in the Windows Context Menu.
/// </summary>
public record MenuDefinition
{
    /// <summary>
    /// The text displayed to the user in the right-click menu.
    /// </summary>
    public required string MenuItemName { get; init; }

    /// <summary>
    /// An optional absolute path to an .ico file to display next to the menu item.
    /// </summary>
    public string? IconPath { get; init; }

    /// <summary>
    /// Determines the sorting order of this item relative to other items in the same menu.
    /// Lower numbers appear higher in the list.
    /// </summary>
    public required int Ordinal { get; init; }

    /// <summary>
    /// Defines which sub-menu (if any) this item should be nested under.
    /// </summary>
    public MenuPlacement Placement { get; init; } = MenuPlacement.Root;

    /// <summary>
    /// Optional child menus. If populated, clicking this item will expand a sub-menu 
    /// instead of immediately executing logic.
    /// </summary>
    public IEnumerable<MenuDefinition>? SubItems { get; init; }

    /// <summary>
    /// If true, the JCMU Core will execute this addon in the background without 
    /// showing a console window. If false, a console window will appear.
    /// </summary>
    public bool RunInBackground { get; init; } = false;

    /// <summary>
    /// Validates the menu definition and its sub-items recursively.
    /// </summary>
    /// <returns>A parameterless monad representing the validation result.</returns>
    public Maybe Validate()
    {
        if (string.IsNullOrWhiteSpace(MenuItemName))
            return Maybe.Fail("MenuItemName cannot be null or whitespace.");

        if (SubItems == null || !SubItems.Any())
            return Maybe.SUCCESS;

        // Evaluates all validations and returns a single Success Maybe 
        // ONLY if every item in the sequence succeeded. 
        // (If one fails, Sequence() returns that specific failure).
        return SubItems
            .Select(sub => sub.Validate())
            .Sequence();
    }
}
