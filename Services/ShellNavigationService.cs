namespace InventoryApp.Services;

/// <summary>
/// Production implementation of INavigationService, backed by .NET MAUI Shell.
/// This is the ONLY class in the app allowed to reference Shell.Current.
/// </summary>
public class ShellNavigationService : INavigationService
{
    public Task GoToDetailAsync(int itemId)
        => Shell.Current.GoToAsync($"detail?itemId={itemId}");

    public Task GoBackAsync()
        => Shell.Current.GoToAsync("..");
}
