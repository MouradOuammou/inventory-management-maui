namespace InventoryApp.Models;

/// <summary>
/// Represents a single inventory record as returned by the REST API.
/// This is a pure data object (POCO): it carries no business logic,
/// no UI logic and no service dependency. Keeping models "dumb" makes
/// them safe to serialize/deserialize and easy to unit test in isolation.
/// </summary>
public class InventoryItem
{
    /// <summary>Unique identifier of the item, used as the route key for navigation
    /// and as the resource id for the PUT /api/inventory/{id} call.</summary>
    public int ItemId { get; set; }

    /// <summary>Human-readable name displayed in the list and detail pages.</summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>Current stock quantity. This is the only field the user is allowed to edit.</summary>
    public int CurrentQuantity { get; set; }

    /// <summary>Timestamp of the last update, set server-side after a successful PUT.</summary>
    public DateTime LastUpdated { get; set; }
}
