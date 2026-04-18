using WebpKiller.Settings;

namespace WebpKiller;

public partial class FrmSettings : Form
{
    private bool HasSelection => LbFolders.SelectedIndex >= 0;

    private FolderSettingsViewModel CurrentFolder
    {
        get
        {
            return (FolderSettingsViewModel?)LbFolders.SelectedItem
                ?? throw new InvalidOperationException("No folder is selected");
        }
        set
        {
            LbFolders.Items[LbFolders.SelectedIndex] = value;
        }
    }

    private FolderSettingsViewModel[] AllFolders
    {
        get
        {
            return [.. LbFolders.Items.OfType<FolderSettingsViewModel>()];
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            LbFolders.SuspendLayout();
            try
            {
                LbFolders.Items.Clear();
                LbFolders.Items.AddRange([.. value]);
            }
            finally
            {
                LbFolders.ResumeLayout();
            }
        }
    }

    public FrmSettings()
    {
        InitializeComponent();
        Icon = Resources.GetApplicationIcon();
        AllFolders = [.. SettingsProvider.GetSettings().Settings.Select(m => new FolderSettingsViewModel(m))];
        foreach (var control in Controls.OfType<CheckBox>())
        {
            control.CheckedChanged += UpdateSettingOptions;
            control.Enabled = false;
        }
    }

    private void UpdateSettingOptions(object? sender, EventArgs e)
    {
        if (!HasSelection)
        {
            return;
        }
        var current = CurrentFolder;
        current.Settings = current.Settings with
        {
            DeleteWebp = CbDeleteWebp.Checked,
            Enabled = CbEnabled.Checked,
            Recursive = CbRecursive.Checked,
            ScanOnStart = CbScanOnStartup.Checked,
            ShowConversionMsg = CbShowConversionMsg.Checked
        };
        CurrentFolder = current;
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        foreach (var item in AllFolders)
        {
            try
            {
                item.Validate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Settings are not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        var settings = new JsonSettingsV1(1, [.. AllFolders.Select(m => m.Settings)]);
        try
        {
            SettingsProvider.SaveSettings(settings);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Failed to save settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
        AutoMonitor.Start(settings.Settings);
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (FBD.ShowDialog() == DialogResult.OK)
        {
            var selected = FBD.SelectedPath;

            var dirs = AllFolders.Select(m => m.Settings.Folder).ToArray();

            if (dirs.Contains(selected, StringComparer.InvariantCultureIgnoreCase))
            {
                MessageBox.Show("The selected folder is already being monitored", "Duplicate folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dirs.Any(m => m.StartsWith(selected + Path.DirectorySeparatorChar, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!ComplexDialogs.ShowHelpYesNo("A subfolder of the selected directory is already being monitored. This can cause problems. Continue anyways?", "Subfolder already monitored", "You can safely add this folder if you disable the 'include subdirectories' option on it, otherwise a webp file in the child folder will cause two conversion events to trigger."))
                {
                    return;
                }
            }
            if (dirs.Any(m => selected.StartsWith(m + Path.DirectorySeparatorChar, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!ComplexDialogs.ShowHelpYesNo("A parent directory of the selected folder is already being monitored. This can cause problems. Continue anyways?", "Parent folder already monitored", "You can safely add this folder if you disable the 'include subdirectories' option on the parent, otherwise a webp file in the child folder will cause two conversion events to trigger."))
                {
                    return;
                }
            }
            var vm = new FolderSettingsViewModel(new FolderSettingsV1(selected, true, true, true, true, true));
            LbFolders.SelectedIndex = LbFolders.Items.Add(vm);
        }
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        var index = LbFolders.SelectedIndex;
        if (index >= 0)
        {
            LbFolders.Items.RemoveAt(index);
            if (LbFolders.Items.Count > 0)
            {
                LbFolders.SelectedIndex = Math.Clamp(index, 0, LbFolders.Items.Count - 1);
            }
        }
        else
        {
            MessageBox.Show("Please select an item from the list first", "No folder selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }

    private void LbFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
        var cb = Controls.OfType<CheckBox>().ToList();
        cb.ForEach(m => m.Enabled = LbFolders.SelectedIndex >= 0);
        if (LbFolders.SelectedIndex >= 0)
        {
            var current = CurrentFolder.Settings;
            CbEnabled.Checked = current.Enabled;
            CbScanOnStartup.Checked = current.ScanOnStart;
            CbDeleteWebp.Checked = current.DeleteWebp;
            CbRecursive.Checked = current.Recursive;
            CbShowConversionMsg.Checked = current.ShowConversionMsg;
        }
    }
}
