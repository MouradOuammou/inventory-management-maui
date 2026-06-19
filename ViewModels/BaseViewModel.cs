using CommunityToolkit.Mvvm.ComponentModel;

namespace InventoryApp.ViewModels;

/// <summary>
/// Common base class for all ViewModels. Centralizes cross-cutting
/// presentation concerns (busy state, page title, error message) so
/// concrete ViewModels only contain their own business logic.
/// </summary>
public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    partial void OnErrorMessageChanged(string? value)
    {
        OnPropertyChanged(nameof(HasError));
    }

    protected void ClearError() => ErrorMessage = null;
}
