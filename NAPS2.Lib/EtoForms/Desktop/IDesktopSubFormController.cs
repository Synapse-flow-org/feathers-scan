namespace NAPS2.EtoForms.Desktop;

public interface IDesktopSubFormController
{
    IDesktopSubFormController WithSelection(Func<ListSelection<UiImage>> selectionFunc);
    void ShowCropForm();
    void ShowBrightnessContrastForm();
    void ShowHueSaturationForm();
    void ShowBlackWhiteForm();
    void ShowSharpenForm();
    void ShowSplitForm();
    void ShowCombineForm();
    void ShowRotateForm();
    void ShowProfilesForm();
    void ShowCreateFolderForm();
    void ShowBrandForm();
    void ShowOcrForm();
    void ShowBatchScanForm();
    void ShowScannerSharingForm();
    void ShowViewerForm();
    void ShowTypageForm();
    void SetAsPsuite();
    void ShowPdfSettingsForm();
    void ShowImageSettingsForm();
    void ShowEmailSettingsForm();
    void ShowAboutForm();
    void ShowSettingsForm();
}