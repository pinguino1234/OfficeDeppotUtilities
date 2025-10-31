using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using OfficeDeppotUtiliteies.ViewModels;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using ReactiveUI.Avalonia;
using ReactiveUI;
using SkiaSharp;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;

namespace OfficeDeppotUtiliteies.Views
{
    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }

        async void OpenFile(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var storage = topLevel?.StorageProvider;

            if (storage != null) 
            {
                var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
                    FileTypeFilter = [FilePickerFileTypes.Pdf]
                });

                foreach (var file in files)
                {
                    await using var stream = await file.OpenReadAsync();

                    // Copiar a MemoryStream (opcional, pero recomendable)
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    ms.Position = 0;

                    // Cargar PDF desde el MemoryStream
                    var document = PdfReader.Open(ms, PdfDocumentOpenMode.Modify);

                    ViewModel!.ListOfPdfs.Add(new PdfDataViewModel
                    {
                        FileName = file.Name.Replace(".pdf", ""),
                        PageCount = $"{document.PageCount} Paginas",
                        PdfData = ms
                    });

                    ViewModel!.AreItemsAdded = true;
                }
            }
        }

        async void SaveFile(object? sender, RoutedEventArgs e)
        {
            if (!ViewModel!.AreItemsAdded) return;
            
            var topLevel = TopLevel.GetTopLevel(this);
            var storage = topLevel?.StorageProvider;

            if (storage != null)
            {
                var options = new FilePickerSaveOptions
                {
                    Title = "Save PDF File",
                    SuggestedFileName = "document.pdf",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("PDF Documents")
                        {
                            Patterns = new[] { "*.pdf" },
                            MimeTypes = new[] { "application/pdf" }
                        }
                    }
                };

                var file = await storage.SaveFilePickerAsync(options);
                
                if (file != null)
                {
                    foreach (PdfDataViewModel pdf in ViewModel!.ListOfPdfs)
                    {
                        // Ignorar el nombre elegido por el usuario
                        var folder = await file.GetParentAsync();

                        // Tu nombre de archivo ya definido
                        string nombreFinal = pdf.FileName + "_rotado.pdf";

                        // Crear o sobrescribir el archivo
                        var finalFile = await folder!.CreateFileAsync(nombreFinal);

                        //Clonar contenido cerrado
                        var bytes = pdf.PdfData!.ToArray(); // clona los datos
                        var nms = new MemoryStream(bytes); // nuevo stream, abierto

                        //Rotamos archivo
                        var rotated = ViewModel!.ChangeRotation.Execute(nms).ToTask().Result;

                        // Escribir contenido
                        await using var stream = await finalFile!.OpenWriteAsync();
                        await rotated!.CopyToAsync(stream);
                    }
                }

            }
        }
        void OnToggleClick(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton clicked)
            {
                // Desactivar todos los demás ToggleButtons del mismo contenedor
                var parent = (StackPanel)clicked.Parent!;
                foreach (var child in parent.Children.OfType<ToggleButton>())
                {
                    if (child != clicked)
                        child.IsChecked = false;
                }

                // Si el botón ya estaba activo, mantenerlo activo
                clicked.IsChecked = true;
            }
        }
    }
}