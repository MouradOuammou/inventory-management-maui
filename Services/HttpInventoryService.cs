using System.Net.Http.Json;
using InventoryApp.Models;

namespace InventoryApp.Services;

/// <summary>
/// Real REST API implementation of IInventoryService, using HttpClient.
/// This is the production-ready counterpart to MockInventoryService —
/// both implement the exact same contract, so the rest of the app
/// (ViewModels, Views, DI) never needs to know which one is active.
///
/// To activate it: in MauiProgram.cs, replace
///   AddSingleton&lt;IInventoryService, MockInventoryService&gt;()
/// with
///   AddSingleton&lt;IInventoryService, HttpInventoryService&gt;()
/// </summary>
public class HttpInventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;

    public HttpInventoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Example: _httpClient.BaseAddress = new Uri("https://api.example.com/");
    }

    public async Task<List<InventoryItem>> GetItemsAsync()
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<InventoryItem>>("api/inventory");
            return items ?? new List<InventoryItem>();
        }
        catch (HttpRequestException ex)
        {
            throw new InventoryServiceException("Could not reach the inventory API.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new InventoryServiceException("The request timed out.", ex);
        }
    }

    public async Task<InventoryItem?> GetItemByIdAsync(int itemId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<InventoryItem>($"api/inventory/{itemId}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            throw new InventoryServiceException("Could not reach the inventory API.", ex);
        }
    }

    public async Task<InventoryItem> UpdateQuantityAsync(int itemId, int newQuantity)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/inventory/{itemId}",
                new { CurrentQuantity = newQuantity });

            if (!response.IsSuccessStatusCode)
            {
                throw new InventoryServiceException($"API returned status code {(int)response.StatusCode}.");
            }

            var updated = await response.Content.ReadFromJsonAsync<InventoryItem>();
            return updated ?? throw new InventoryServiceException("API returned an empty response.");
        }
        catch (HttpRequestException ex)
        {
            throw new InventoryServiceException("Could not reach the inventory API.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new InventoryServiceException("The request timed out.", ex);
        }
    }
}
