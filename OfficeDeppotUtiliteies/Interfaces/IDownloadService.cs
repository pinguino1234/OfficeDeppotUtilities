using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeppotUtiliteies.Interfaces
{
    public interface IFileDownloader
    {
        Task DownloadAsync(string filename, byte[] data, string mimeType);
    }
}
