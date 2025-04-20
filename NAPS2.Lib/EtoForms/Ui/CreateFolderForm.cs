using Eto.Drawing;
using Eto.Forms;
using NAPS2.EtoForms.Layout;
using NAPS2.Folder;
using NAPS2.Scan;

namespace NAPS2.EtoForms.Ui;

public class CreateFolderForm : EtoDialogBase
{
    private readonly ErrorOutput _errorOutput;
    private readonly TextBox _folderNum = new();
    private readonly TextBox _folderFirstName = new();
    private readonly TextBox _folderLastName = new();
    private readonly TextBox _folderCIN = new();

    private bool _result;
    private FolderConfig _folderConfig;

    public CreateFolderForm(Naps2Config config, ErrorOutput errorOutput, FolderConfig folderConfig) : base(config)
    {
        Title = string.IsNullOrEmpty(folderConfig.Num) ? UiStrings.NewFolder : UiStrings.UpdateFolder;
        IconName = string.IsNullOrEmpty(folderConfig.Num) ? "add_folder" : "update_folder";
        _errorOutput = errorOutput;
        _folderConfig = folderConfig;
    }

    protected override void BuildLayout()
    {
        InitFolderConfig();
        FormStateController.DefaultExtraLayoutSize = new Size(60, 0);
        FormStateController.FixedHeightLayout = true;

        LayoutController.Content = L.Column(
            C.Label("Numéro Dossier"),
            _folderNum,
            C.Label("Nom"),
            _folderFirstName,
            C.Label("Prénom"),
            _folderLastName,
            C.Label("CIN"),
            _folderCIN,
            C.Spacer(),
            C.Spacer(),
            L.Row(
                C.Filler(),
                L.OkCancel(
                    C.OkButton(this, SaveFolder),
                    C.CancelButton(this))
            )
        );
    }

    public bool Result => _result;
    public FolderConfig? FolderConfig => _folderConfig;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    private void InitFolderConfig()
    {
        _folderNum.Text = _folderConfig.Num;
        _folderFirstName.Text = _folderConfig.FirstName;
        _folderLastName.Text = _folderConfig.LastName;
        _folderCIN.Text = _folderConfig.CIN;
    }

    private bool SaveFolder()
    {
        if (string.IsNullOrEmpty(_folderNum.Text))
        {
            _errorOutput.DisplayError("Numéro Dossier manquant");
            return false;
        }
        _result = true;
        _folderConfig = new FolderConfig()
        {
            Num = _folderNum.Text,
            FirstName = _folderFirstName.Text,
            LastName = _folderLastName.Text,
            CIN = _folderCIN.Text
        };
        return true;
    }
}