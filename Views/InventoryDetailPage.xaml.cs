using InventoryApp.ViewModels;

namespace InventoryApp.Views;

/// <summary>
/// Code-behind kept minimal by design. Data loading is triggered by the
/// ViewModel itself in ApplyQueryAttributes as soon as Shell provides the id.
/// </summary>
public partial class InventoryDetailPage : ContentPage
{
    public InventoryDetailPage(InventoryDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
