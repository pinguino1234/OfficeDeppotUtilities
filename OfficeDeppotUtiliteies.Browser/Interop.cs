using OfficeDeppotUtiliteies.Interfaces;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace OfficeDeppotUtiliteies.Browser;

/*
 * Por alguna razon la descarga simulada no comienza
 * Se dejara el codigo pertinente para futuras correciones
 */
public partial class BrowserFileDownloader : IFileDownloader
{
    [JSImport("downloadFile", "main.js")]
    static partial void DownloadFile(string filename, byte[] data, string mimeType);

    public Task DownloadAsync(string filename, byte[] data, string mimeType)
    {
        DownloadFile(filename, data, mimeType);
        return Task.CompletedTask;
    }
}
