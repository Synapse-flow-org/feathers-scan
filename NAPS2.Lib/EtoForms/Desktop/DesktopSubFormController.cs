using NAPS2.EtoForms.Ui;
using NAPS2.Folder;
using NAPS2.Logging;
using NAPS2.Ocr;

namespace NAPS2.EtoForms.Desktop;

public class DesktopSubFormController : IDesktopSubFormController
{
    private readonly IFormFactory _formFactory;
    private readonly UiImageList _imageList;
    private readonly FolderConfig _folderConfig;
    private readonly DesktopImagesController _desktopImagesController;
    private readonly TesseractLanguageManager _tesseractLanguageManager;
    private readonly ErrorOutput _errorOutput;

    public DesktopSubFormController(IFormFactory formFactory, UiImageList imageList, FolderConfig folderConfig,
        DesktopImagesController desktopImagesController, TesseractLanguageManager tesseractLanguageManager, ErrorOutput errorOutput)
    {
        _formFactory = formFactory;
        _imageList = imageList;
        _folderConfig = folderConfig;
        _desktopImagesController = desktopImagesController;
        _tesseractLanguageManager = tesseractLanguageManager;
        _errorOutput = errorOutput;
    }

    private Func<ListSelection<UiImage>>? SelectionFunc { get; init; }

    private ListSelection<UiImage> Selection => SelectionFunc?.Invoke() ?? _imageList.Selection;

    public IDesktopSubFormController WithSelection(Func<ListSelection<UiImage>> selectionFunc)
    {
        return new DesktopSubFormController(_formFactory, _imageList, _folderConfig, _desktopImagesController,
            _tesseractLanguageManager, _errorOutput)
        {
            SelectionFunc = selectionFunc
        };
    }

    public void ShowCropForm() => ShowImageForm<CropForm>();
    public void ShowBrightnessContrastForm() => ShowImageForm<BrightContForm>();
    public void ShowHueSaturationForm() => ShowImageForm<HueSatForm>();
    public void ShowBlackWhiteForm() => ShowImageForm<BlackWhiteForm>();
    public void ShowSharpenForm() => ShowImageForm<SharpenForm>();
    public void ShowSplitForm() => ShowImageForm<SplitForm>();
    public void ShowRotateForm() => ShowImageForm<RotateForm>();

    public void ShowCombineForm()
    {
        if (_imageList.Images.Count < 2) return;
        ShowImageForm<CombineForm>();
    }

    private void ShowImageForm<T>() where T : ImageFormBase
    {
        var selection = Selection;
        if (selection.Any())
        {
            var form = _formFactory.Create<T>();
            form.Image = selection.First();
            form.SelectedImages = selection.ToList();
            form.ShowModal();
        }
    }

    public void ShowProfilesForm()
    {
        var form = _formFactory.Create<ProfilesForm>();
        form.ImageCallback = _desktopImagesController.ReceiveScannedImage();
        form.ShowModal();
    }

    public void ShowCreateFolderForm()
    {
        var createFolderForm = _formFactory.Create<CreateFolderForm>();
        createFolderForm.ShowModal();
        if (!createFolderForm.Result)
        {
            return;
        }

        _folderConfig.Num = createFolderForm?.FolderConfig?.Num;
        _folderConfig.FirstName = createFolderForm?.FolderConfig?.FirstName;
        _folderConfig.LastName = createFolderForm?.FolderConfig?.LastName;
        _folderConfig.CIN = createFolderForm?.FolderConfig?.CIN;
        _folderConfig.FolderConfigUpdate();
    }

    public void ShowOcrForm()
    {
        if (_tesseractLanguageManager.InstalledLanguages.Any())
        {
            _formFactory.Create<OcrSetupForm>().ShowModal();
        }
        else
        {
            _formFactory.Create<OcrDownloadForm>().ShowModal();
            if (_tesseractLanguageManager.InstalledLanguages.Any())
            {
                _formFactory.Create<OcrSetupForm>().ShowModal();
            }
        }
    }

    public void ShowBatchScanForm()
    {
        var form = _formFactory.Create<BatchScanForm>();
        form.ImageCallback = _desktopImagesController.ReceiveScannedImage();
        form.ShowModal();
    }

    public void ShowScannerSharingForm()
    {
        var form = _formFactory.Create<ScannerSharingForm>();
        form.ShowModal();
    }

    public void ShowViewerForm()
    {
        var selected = Selection.FirstOrDefault();
        if (selected != null)
        {
            using var viewer = _formFactory.Create<PreviewForm>();
            viewer.CurrentImage = selected;
            viewer.ShowModal();
        }
    }

    public void ShowTypageForm()
    {
        if (string.IsNullOrEmpty(_folderConfig.Num))
        {
            _errorOutput.DisplayError(UiStrings.FolderNumError);
            return;
        }

        var selected = Selection.FirstOrDefault();
        if (selected != null)
        {
            var typeForm = _formFactory.Create<TypageForm>();
            typeForm.ShowModal();
            if (!typeForm.Result)
            {
                return;
            }

            selected.Typage = new() { Code = typeForm.Typage?.Code };
            var aTyper = _imageList.Images.IndexOf(selected);


            var newSelection = new List<UiImage>() { selected };
            for (int i = aTyper - 1; i > 0; i -= 1) // Incrémente de 2 en 2
            {
                if (string.IsNullOrEmpty(_imageList.Images[i].Typage?.Code))
                {
                    _imageList.Images[i].Typage = new() { Code = "PSUITE" };
                    newSelection.Add(_imageList.Images[i]);
                }
                else
                {
                    break;
                }
            }

            _imageList.UpdateSelection(ListSelection.From(newSelection));
        }
    }

    public void SetAsPsuite()
    {
        if (string.IsNullOrEmpty(_folderConfig.Num))
        {
            _errorOutput.DisplayError(UiStrings.FolderNumError);
            return;
        }

        var selected = Selection.FirstOrDefault();

        if (selected != null)
        {
            var index = _imageList.Images.IndexOf(selected);
            if (index == 0)
            {
                return;
            }

            selected.Typage = new() { Code = "PSUITE" };
            var newSelection = new List<UiImage>() { _imageList.Images[index + 1 < _imageList.Images.Count ? index + 1 : 0] };
            _imageList.UpdateSelection(ListSelection.From(newSelection));
        }
    }

    public void ShowPdfSettingsForm()
    {
        _formFactory.Create<PdfSettingsForm>().ShowModal();
    }

    public void ShowImageSettingsForm()
    {
        _formFactory.Create<ImageSettingsForm>().ShowModal();
    }

    public void ShowEmailSettingsForm()
    {
        _formFactory.Create<EmailSettingsForm>().ShowModal();
    }

    public void ShowSettingsForm()
    {
        _formFactory.Create<SettingsForm>().ShowModal();
    }

    public void ShowAboutForm()
    {
        _formFactory.Create<AboutForm>().ShowModal();
    }

    public void ShowBrandForm()
    {
        return;
    }
}