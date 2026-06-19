using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels;

/// <summary>
/// ViewModel backing the inventory list page. Depends ONLY on the
/// IInventoryService abstraction (constructor injection), never on a
/// concrete implementation — this is what makes it testable and scalable.
/// </summary>
public partial class InventoryListViewModel : BaseViewModel
{
    private readonly IInventoryService _inventoryService;

    public ObservableCollection<InventoryItem> Items { get; } = new();

    public InventoryListViewModel(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
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
        await Shell.Current.GoToAsync($"detail?itemId={selectedItem.ItemId}");
    }
}
