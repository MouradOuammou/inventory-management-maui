namespace InventoryApp;

/// <summary>
/// Application root. Sets the Shell (AppShell) as the main page.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
