using Avalonia;
using Avalonia.Browser;
using OfficeDeppotUtiliteies;
using OfficeDeppotUtiliteies.Browser;
using OfficeDeppotUtiliteies.Interfaces;
using ReactiveUI.Avalonia;
using Splat;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;

//await JSHost.ImportAsync("main", "./main.js");

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
            .WithInterFont()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
    {
        Locator.CurrentMutable.Register<IFileDownloader>(() => new BrowserFileDownloader());
        return AppBuilder.Configure<App>();
    }
}