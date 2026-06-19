using InventoryApp.Services;

namespace InventoryApp.Tests;

/// <summary>
/// Test double for INavigationService. Records the last navigation call
/// instead of touching Shell, so tests can assert on navigation intent
/// without any MAUI runtime.
/// </summary>
public class FakeNavigationService : INavigationService
{
    public int? LastNavigatedItemId { get; private set; }
    public bool WentBack { get; private set; }

    public Task GoToDetailAsync(int itemId)
    {
        LastNavigatedItemId = itemId;
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        WentBack = true;
        return Task.CompletedTask;
    }
}
