using InventoryApp.Models;

namespace InventoryApp.Services;

/// <summary>
/// Defines the contract for inventory data access, independent of the
/// underlying implementation (REST API, mock, local DB, etc.).
///
/// This abstraction is the cornerstone of the architecture:
///   - ViewModels depend ONLY on this interface (Dependency Inversion Principle).
///   - Implementations can be swapped (Mock today, real HttpClient tomorrow)
///     without touching a single line of UI or ViewModel code.
///   - Unit tests can inject a fake/mock implementation to isolate
///     ViewModel logic from any network or storage concern.
/// </summary>
public interface IInventoryService
{
    /// <summary>
    /// Retrieves the full list of inventory items.
    /// Maps to GET /api/inventory.
    /// </summary>
    Task<List<InventoryItem>> GetItemsAsync();

    /// <summary>
    /// Retrieves a single inventory item by its identifier.
    /// Used to populate the detail page without re-fetching the whole list.
    /// </summary>
    Task<InventoryItem?> GetItemByIdAsync(int itemId);

    /// <summary>
    /// Updates the stock quantity of a given item.
    /// Maps to PUT /api/inventory/{id}.
    /// </summary>
    Task<InventoryItem> UpdateQuantityAsync(int itemId, int newQuantity);
}

/// <summary>
/// Custom exception type so that ViewModels can catch a single, well-known
/// exception type instead of leaking HttpRequestException/TaskCanceledException
/// (transport-level concerns) into the presentation layer.
/// </summary>
public class InventoryServiceException : Exception
{
    public InventoryServiceException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
