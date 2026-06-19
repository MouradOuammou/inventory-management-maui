using InventoryApp.Services;
using InventoryApp.ViewModels;
using InventoryApp.Views;
using Microsoft.Extensions.Logging;

namespace InventoryApp;

/// <summary>
/// DI container configuration. To switch to a real API later, only
/// ONE line below needs to change (IInventoryService binding).
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IInventoryService, MockInventoryService>();

        builder.Services.AddTransient<InventoryListViewModel>();
        builder.Services.AddTransient<InventoryDetailViewModel>();

        builder.Services.AddTransient<InventoryListPage>();
        builder.Services.AddTransient<InventoryDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
