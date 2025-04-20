using Eto.Drawing;
using Eto.Forms;
using NAPS2.EtoForms.Layout;
using NAPS2.EtoForms.Widgets;
using NAPS2.Folder;
using NAPS2.Scan;

namespace NAPS2.EtoForms.Ui;

public class TypageForm : EtoDialogBase
{
    private readonly ErrorOutput _errorOutput;
    private readonly TextBox _folderNum = new();
    private readonly DropDownWidget<Typage> _types = new();

    private bool _result;
    private Typage _typage;

    public TypageForm(Naps2Config config, ErrorOutput errorOutput) : base(config)
    {
        Title = "Typage document"; /* UiStrings.EditProfileFormTitle;*/
        IconName = "typage_box";
        _errorOutput = errorOutput;
        _types.Format = x => x.Code ?? "";
        FillTypageList();
    }

    private void FillTypageList()
    {
        var types = Config.Get(x => x.TypageList);
        if (types != null && types.Count > 0)
        {
            _types.Items = types;
        }
        //_onboardingVis.IsVisible = _profileManager.Profiles.Count == 0;
    }

    protected override void BuildLayout()
    {
        FormStateController.DefaultExtraLayoutSize = new Size(60, 0);
        FormStateController.FixedHeightLayout = true;

        LayoutController.Content = L.Column(
            C.Label("Type"),
            _types,
            C.Spacer(),
            C.Spacer(),
            L.Row(
                C.Filler(),
                L.OkCancel(
                    C.OkButton(this, SaveTypage),
                    C.CancelButton(this))
            )
        );
    }

    public bool Result => _result;
    public Typage? Typage => _typage;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    private bool SaveTypage()
    {
        if (string.IsNullOrEmpty(_types.SelectedItem?.Code))
        {
            _errorOutput.DisplayError(MiscResources.NameMissing);
            return false;
        }
        _result = true;
        _typage = _types.SelectedItem;
        return true;
    }
}