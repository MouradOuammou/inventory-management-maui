using InventoryApp.Models;

namespace InventoryApp.Services;

/// <summary>
/// In-memory implementation of <see cref="IInventoryService"/>.
/// Simulates a real REST API client: artificial network latency,
/// async data, and the ability to simulate failure scenarios.
/// In production, this would be replaced by an HttpInventoryService
/// implementing the SAME interface — no other layer would need to change.
/// </summary>
public class MockInventoryService : IInventoryService
{
    private const int SimulatedNetworkDelayMs = 600;
    private readonly List<InventoryItem> _items;

    public MockInventoryService()
    {
        _items = new List<InventoryItem>
        {
            new() { ItemId = 1, ItemName = "Laptop Dell Latitude 5430", CurrentQuantity = 12, LastUpdated = DateTime.Now.AddDays(-2) },
            new() { ItemId = 2, ItemName = "Wireless Mouse Logitech MX",  CurrentQuantity = 45, LastUpdated = DateTime.Now.AddDays(-5) },
            new() { ItemId = 3, ItemName = "USB-C Docking Station",       CurrentQuantity = 8,  LastUpdated = DateTime.Now.AddDays(-1) },
            new() { ItemId = 4, ItemName = "27\" Monitor Dell UltraSharp", CurrentQuantity = 3, LastUpdated = DateTime.Now.AddHours(-10) },
            new() { ItemId = 5, ItemName = "Mechanical Keyboard",          CurrentQuantity = 0, LastUpdated = DateTime.Now.AddDays(-7) },
        };
    }

    public async Task<List<InventoryItem>> GetItemsAsync()
    {
        await SimulateNetworkCallAsync();
        return _items.Select(CloneItem).ToList();
    }

    public async Task<InventoryItem?> GetItemByIdAsync(int itemId)
    {
        await SimulateNetworkCallAsync();
        var item = _items.FirstOrDefault(i => i.ItemId == itemId);
        return item is null ? null : CloneItem(item);
    }

    public async Task<InventoryItem> UpdateQuantityAsync(int itemId, int newQuantity)
    {
        await SimulateNetworkCallAsync();

        if (newQuantity < 0)
            throw new InventoryServiceException("Quantity cannot be negative.");

        var item = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (item is null)
            throw new InventoryServiceException($"Item with id {itemId} was not found.");

        item.CurrentQuantity = newQuantity;
        item.LastUpdated = DateTime.Now;
        return CloneItem(item);
    }

    private static async Task SimulateNetworkCallAsync() => await Task.Delay(SimulatedNetworkDelayMs);

    private static InventoryItem CloneItem(InventoryItem source) => new()
    {
        ItemId = source.ItemId,
        ItemName = source.ItemName,
        CurrentQuantity = source.CurrentQuantity,
        LastUpdated = source.LastUpdated
    };
}
