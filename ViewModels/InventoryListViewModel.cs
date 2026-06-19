using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels;

/// <summary>
/// ViewModel backing the inventory list page. Depends only on
/// IInventoryService and INavigationService — both abstractions —
/// so this class has zero dependency on the MAUI runtime and can be
/// instantiated and tested as a plain .NET object.
/// </summary>
public partial class InventoryListViewModel : BaseViewModel
{
    private readonly IInventoryService _inventoryService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<InventoryItem> Items { get; } = new();

    public InventoryListViewModel(IInventoryService inventoryService, INavigationService navigationService)
    {
        _inventoryService = inventoryService;
        _navigationService = navigationService;
        Title = "Inventory";
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var items = await _inventoryService.GetItemsAsync();

            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        }
        catch (InventoryServiceException ex)
        {
            ErrorMessage = $"Unable to load inventory: {ex.Message}";
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred while loading inventory. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailAsync(InventoryItem? selectedItem)
    {
        if (selectedItem is null) return;
        await _navigationService.GoToDetailAsync(selectedItem.ItemId);
    }
}
