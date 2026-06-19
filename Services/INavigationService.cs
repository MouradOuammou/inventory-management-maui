namespace InventoryApp.Services;

/// <summary>
/// Abstraction over page navigation. ViewModels depend on this interface
/// instead of calling Shell.Current directly, which removes the last MAUI
/// runtime dependency from the ViewModel layer and makes navigation
/// fully unit-testable via a fake implementation.
/// </summary>
public interface INavigationService
{
    Task GoToDetailAsync(int itemId);
    Task GoBackAsync();
}
