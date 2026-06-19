using InventoryApp.Views;

namespace InventoryApp;

/// <summary>
/// Defines navigable routes. "detail" is registered explicitly since
/// it's reached only via navigation from the list, never directly.
/// </summary>
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("detail", typeof(InventoryDetailPage));
    }
}
