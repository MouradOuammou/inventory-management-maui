using InventoryApp.ViewModels;
using Xunit;

namespace InventoryApp.Tests;

/// <summary>
/// Unit tests for InventoryDetailViewModel. As with the list ViewModel,
/// the only dependency (IInventoryService) is faked, so these run as
/// plain .NET object tests with no MAUI runtime involved.
///
/// _itemId is intentionally private and only set via ApplyQueryAttributes
/// (the same entry point Shell uses in production), so tests go through
/// that method rather than reaching into private state.
/// </summary>
public class InventoryDetailViewModelTests
{
    private static InventoryDetailViewModel CreateViewModel(
        FakeInventoryService fakeService, int itemId)
    {
        var viewModel = new InventoryDetailViewModel(fakeService);
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            ["itemId"] = itemId
        });
        return viewModel;
    }

    [Fact]
    public async Task ApplyQueryAttributes_LoadsItem_OnSuccess()
    {
        var fakeService = new FakeInventoryService();
        var viewModel = CreateViewModel(fakeService, itemId: 1);

        // CreateViewModel already triggers a fire-and-forget LoadItemAsync via
        // ApplyQueryAttributes. Awaiting the command again here is redundant
        // but harmless (idempotent fake), and gives a deterministic await point.
        await viewModel.LoadItemCommand.ExecuteAsync(null);

        Assert.Equal("Test Item A", viewModel.ItemName);
        Assert.Equal(10, viewModel.CurrentQuantity);
        Assert.False(viewModel.HasError);
    }

    [Fact]
    public async Task LoadItemCommand_SetsErrorMessage_WhenItemNotFound()
    {
        var fakeService = new FakeInventoryService();
        var viewModel = CreateViewModel(fakeService, itemId: 999);

        await viewModel.LoadItemCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.Contains("could not be found", viewModel.ErrorMessage);
    }

    [Fact]
    public async Task LoadItemCommand_SetsErrorMessage_WhenServiceThrows()
    {
        var fakeService = new FakeInventoryService { ShouldThrowOnGetById = true };
        var viewModel = CreateViewModel(fakeService, itemId: 1);

        await viewModel.LoadItemCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.Contains("Unable to load item", viewModel.ErrorMessage);
    }

    [Fact]
    public async Task SaveCommand_UpdatesQuantityAndSetsSaveSucceeded_OnSuccess()
    {
        var fakeService = new FakeInventoryService();
        var viewModel = CreateViewModel(fakeService, itemId: 1);
        await viewModel.LoadItemCommand.ExecuteAsync(null);

        viewModel.CurrentQuantity = 25;
        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.True(viewModel.SaveSucceeded);
        Assert.False(viewModel.HasError);

        var persisted = await fakeService.GetItemByIdAsync(1);
        Assert.Equal(25, persisted!.CurrentQuantity);
    }

    [Fact]
    public async Task SaveCommand_DoesNotCallService_WhenQuantityIsNegative()
    {
        var fakeService = new FakeInventoryService();
        var viewModel = CreateViewModel(fakeService, itemId: 1);
        await viewModel.LoadItemCommand.ExecuteAsync(null);

        viewModel.CurrentQuantity = -5;
        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.False(viewModel.SaveSucceeded);
        Assert.Equal("Quantity cannot be negative.", viewModel.ErrorMessage);

        // The underlying item must be untouched since validation should
        // short-circuit before the service is ever called.
        var unchanged = await fakeService.GetItemByIdAsync(1);
        Assert.Equal(10, unchanged!.CurrentQuantity);
    }

    [Fact]
    public async Task SaveCommand_SetsErrorMessage_WhenServiceThrows()
    {
        var fakeService = new FakeInventoryService();
        var viewModel = CreateViewModel(fakeService, itemId: 1);
        await viewModel.LoadItemCommand.ExecuteAsync(null);

        fakeService.ShouldThrowOnUpdate = true;
        viewModel.CurrentQuantity = 30;
        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.False(viewModel.SaveSucceeded);
        Assert.Contains("Unable to save changes", viewModel.ErrorMessage);
    }
}