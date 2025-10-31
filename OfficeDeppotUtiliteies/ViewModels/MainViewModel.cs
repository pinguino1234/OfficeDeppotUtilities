
using Avalonia;
using Avalonia.Controls;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;

namespace OfficeDeppotUtiliteies.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Copyright { get; } = $"\u00A9 {DateTime.Now.Year} Paulino Angulo Martínez. Todos los derechos Reservados";
    public ReactiveCommand<MemoryStream, MemoryStream?> ChangeRotation { get; set; }
    public ReactiveCommand<Unit, Unit> ClearAll { get; set; }
    public ObservableCollection<PdfDataViewModel> ListOfPdfs { get; set; } = [];

    public int evenDegrees { get; set; } = 90;
    public int oddDegrees { get; set; } = 270;

    bool _AreItemsAdded = false;
    public bool AreItemsAdded {
        get => _AreItemsAdded;
        set => this.RaiseAndSetIfChanged(ref _AreItemsAdded, value);
    }

    public MainViewModel()
    {
        ChangeRotation = ReactiveCommand.Create<MemoryStream, MemoryStream?>((ms) =>
        {
            if (Design.IsDesignMode) return null;

            // Crear un nuevo stream de salida
            var output = new MemoryStream();

            // Abrir el PDF original
            using (var document = PdfReader.Open(ms, PdfDocumentOpenMode.Modify))
            {
                for (int i = 0; i < document.Pages.Count; i++)
                {
                    var page = document.Pages[i];
                    page.Rotate = (i % 2 == 0) ? evenDegrees : oddDegrees;
                }

                // Guardar el PDF modificado en el nuevo stream
                document.Save(output, false);
            }

            // Rebobinar el stream para lectura posterior
            output.Position = 0;

            return output;
        });

        ClearAll = ReactiveCommand.Create(() =>
        {
            AreItemsAdded = false;
            ListOfPdfs.Clear();

        });
    }
}
