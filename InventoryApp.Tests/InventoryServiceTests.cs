using InventoryApp.Services;
using Xunit;

namespace InventoryApp.Tests;

public class InventoryServiceTests
{
    [Fact]
    public async Task GetItemsAsync_ReturnsAllSeededItems()
    {
        var service = new FakeInventoryService();
        var items = await service.GetItemsAsync();
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public async Task GetItemsAsync_WhenServiceFails_ThrowsInventoryServiceException()
    {
        var service = new FakeInventoryService { ShouldThrowOnGet = true };
        await Assert.ThrowsAsync<InventoryServiceException>(() => service.GetItemsAsync());
    }

    [Fact]
    public async Task UpdateQuantityAsync_UpdatesQuantityAndTimestamp()
    {
        var service = new FakeInventoryService();
        var before = (await service.GetItemByIdAsync(1))!.LastUpdated;
        var updated = await service.UpdateQuantityAsync(1, 99);
        Assert.Equal(99, updated.CurrentQuantity);
        Assert.True(updated.LastUpdated >= before);
    }

    [Fact]
    public async Task UpdateQuantityAsync_WhenServiceFails_ThrowsInventoryServiceException()
    {
        var service = new FakeInventoryService { ShouldThrowOnUpdate = true };
        await Assert.ThrowsAsync<InventoryServiceException>(() => service.UpdateQuantityAsync(1, 5));
    }

    [Fact]
    public async Task GetItemByIdAsync_WhenItemDoesNotExist_ReturnsNull()
    {
        var service = new FakeInventoryService();
        var result = await service.GetItemByIdAsync(999);
        Assert.Null(result);
    }
}
