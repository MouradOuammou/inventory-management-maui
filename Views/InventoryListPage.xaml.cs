using InventoryApp.ViewModels;

namespace InventoryApp.Views;

/// <summary>
/// Code-behind kept minimal by design: only DI wiring and lifecycle hooks.
/// All business logic lives in InventoryListViewModel.
/// </summary>
public partial class InventoryListPage : ContentPage
{
    private readonly InventoryListViewModel _viewModel;

    public InventoryListPage(InventoryListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadItemsCommand.ExecuteAsync(null);
    }
}
