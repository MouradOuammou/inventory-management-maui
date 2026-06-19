using InventoryApp.Models;
using InventoryApp.ViewModels;
using Xunit;

namespace InventoryApp.Tests;

/// <summary>
/// Unit tests for InventoryListViewModel. Both dependencies (IInventoryService
/// and INavigationService) are fakes — no MAUI runtime, no network, no device.
/// This is the concrete payoff of depending on abstractions everywhere.
/// </summary>
public class InventoryListViewModelTests
{
    [Fact]
    public async Task LoadItemsCommand_PopulatesItems_OnSuccess()
    {
        var fakeService = new FakeInventoryService();
        var fakeNav = new FakeNavigationService();
        var viewModel = new InventoryListViewModel(fakeService, fakeNav);

        await viewModel.LoadItemsCommand.ExecuteAsync(null);

        Assert.Equal(2, viewModel.Items.Count);
        Assert.False(viewModel.HasError);
    }

    [Fact]
    public async Task LoadItemsCommand_SetsErrorMessage_WhenServiceThrows()
    {
        var fakeService = new FakeInventoryService { ShouldThrowOnGet = true };
        var fakeNav = new FakeNavigationService();
        var viewModel = new InventoryListViewModel(fakeService, fakeNav);

        await viewModel.LoadItemsCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.Empty(viewModel.Items);
    }

    [Fact]
    public async Task GoToDetailCommand_CallsNavigationService_WithCorrectId()
    {
        var fakeService = new FakeInventoryService();
        var fakeNav = new FakeNavigationService();
        var viewModel = new InventoryListViewModel(fakeService, fakeNav);
        var item = new InventoryItem { ItemId = 42, ItemName = "Test" };

        await viewModel.GoToDetailCommand.ExecuteAsync(item);

        Assert.Equal(42, fakeNav.LastNavigatedItemId);
    }

    [Fact]
    public async Task GoToDetailCommand_DoesNothing_WhenItemIsNull()
    {
        var fakeService = new FakeInventoryService();
        var fakeNav = new FakeNavigationService();
        var viewModel = new InventoryListViewModel(fakeService, fakeNav);

        await viewModel.GoToDetailCommand.ExecuteAsync(null);

        Assert.Null(fakeNav.LastNavigatedItemId);
    }
}
