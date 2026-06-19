using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Services;

namespace InventoryApp.ViewModels;

/// <summary>
/// ViewModel backing the item detail page. Implements IQueryAttributable
/// to receive the "itemId" navigation parameter from the list page.
/// </summary>
public partial class InventoryDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IInventoryService _inventoryService;
    private int _itemId;

    [ObservableProperty] private string itemName = string.Empty;
    [ObservableProperty] private int currentQuantity;
    [ObservableProperty] private DateTime lastUpdated;
    [ObservableProperty] private bool saveSucceeded;

    public InventoryDetailViewModel(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        Title = "Item Detail";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("itemId", out var value) && int.TryParse(value?.ToString(), out var parsedId))
        {
            _itemId = parsedId;
            _ = LoadItemAsync();
        }
    }

    [RelayCommand]
    private async Task LoadItemAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SaveSucceeded = false;

            var item = await _inventoryService.GetItemByIdAsync(_itemId);
            if (item is null)
            {
                ErrorMessage = "This item could not be found. It may have been removed.";
                return;
            }

            ItemName = item.ItemName;
            CurrentQuantity = item.CurrentQuantity;
            LastUpdated = item.LastUpdated;
        }
        catch (InventoryServiceException ex)
        {
            ErrorMessage = $"Unable to load item: {ex.Message}";
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred while loading the item.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy) return;

        if (CurrentQuantity < 0)
        {
            ErrorMessage = "Quantity cannot be negative.";
            return;
        }

        try
        {
            IsBusy = true;
            ClearError();
            SaveSucceeded = false;

            var updated = await _inventoryService.UpdateQuantityAsync(_itemId, CurrentQuantity);
            LastUpdated = updated.LastUpdated;
            SaveSucceeded = true;
        }
        catch (InventoryServiceException ex)
        {
            ErrorMessage = $"Unable to save changes: {ex.Message}";
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred while saving. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
