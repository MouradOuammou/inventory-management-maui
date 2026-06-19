using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.Tests;

/// <summary>
/// Test double for IInventoryService. Deterministic, no delay, with flags
/// to force failures. Proves that ViewModels can be unit-tested with no
/// MAUI runtime, since they depend only on the interface.
/// </summary>
public class FakeInventoryService : IInventoryService
{
    public bool ShouldThrowOnGet { get; set; }
    public bool ShouldThrowOnUpdate { get; set; }

    private readonly List<InventoryItem> _items = new()
    {
        new InventoryItem { ItemId = 1, ItemName = "Test Item A", CurrentQuantity = 10, LastUpdated = DateTime.Now },
        new InventoryItem { ItemId = 2, ItemName = "Test Item B", CurrentQuantity = 0,  LastUpdated = DateTime.Now },
    };

    public Task<List<InventoryItem>> GetItemsAsync()
    {
        if (ShouldThrowOnGet) throw new InventoryServiceException("Simulated network failure.");
        return Task.FromResult(_items.ToList());
    }

    public Task<InventoryItem?> GetItemByIdAsync(int itemId) =>
        Task.FromResult(_items.FirstOrDefault(i => i.ItemId == itemId));

    public Task<InventoryItem> UpdateQuantityAsync(int itemId, int newQuantity)
    {
        if (ShouldThrowOnUpdate) throw new InventoryServiceException("Simulated update failure.");
        var item = _items.First(i => i.ItemId == itemId);
        item.CurrentQuantity = newQuantity;
        item.LastUpdated = DateTime.Now;
        return Task.FromResult(item);
    }
}
