using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDeppotUtiliteies.ViewModels
{
    public class PdfDataViewModel : ViewModelBase
    {
        string _FileName = "FileName";
        public string FileName {
            get => _FileName;
            set => this.RaiseAndSetIfChanged(ref _FileName, value);
        }

        string _PageCount = "PageCount";
        public string PageCount
        {
            get => _PageCount;
            set => this.RaiseAndSetIfChanged(ref _PageCount, value);
        }

        public MemoryStream? PdfData { get; set; }
    }
}
