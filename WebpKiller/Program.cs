using System.Diagnostics;
using WebpKiller;
using WebpKiller.Settings;

/// <summary>
/// Main entry point
/// </summary>
internal class Program
{
    private static NotifyIcon? icon = null;
    private static readonly Stopwatch cooldown = Stopwatch.StartNew();
    private static bool showForm = false;
    private static bool hasInit = false;

    /// <summary>
    /// Shows a conversion success or failure message
    /// </summary>
    /// <param name="path">Full path of webp file</param>
    /// <param name="result">Conversion result</param>
    /// <remarks>This is only safe to call on the main thread</remarks>
    public static void ReportConversionResult(string path, bool result)
    {
        if (icon != null && cooldown.ElapsedMilliseconds > 5000)
        {
            var text = string.Format(result ?
                "Webp converted to jpg: {0}" :
                "Failed to convert to jpg: {0}",
                Path.GetFileName(path));
            var image = result ? ToolTipIcon.Info : ToolTipIcon.Error;
            icon.ShowBalloonTip(3000, "Image converted", text, image);
            cooldown.Restart();
        }
    }

    [STAThread]
    private static void Main(string[] args)
    {
        if (!DuplicateCheck.Init())
        {
            return;
        }
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Idle += Init;
        Application.Run();
        AutoMonitor.Stop();
        icon?.Dispose();
    }

    private static void Init(object? sender, EventArgs e)
    {
        if (hasInit)
        {
            if (showForm)
            {
                ShowSettingsForm();
            }
            return;
        }
        hasInit = true;

        ContextMenuStrip cms = new();
        cms.Items.Add("Settings").Click += delegate { ShowSettingsForm(); };
        cms.Items.Add(new ToolStripSeparator());
        cms.Items.Add("Exit").Click += delegate { Application.Exit(); };

        icon = new()
        {
            Icon = Resources.GetApplicationIcon(),
            Text = "WebpKiller",
            Visible = true,
            ContextMenuStrip = cms
        };
        icon.DoubleClick += delegate { ShowSettingsForm(); };
        var settings = SettingsProvider.GetSettings();
        var results = settings.Validate();
        if (results.Length != 0)
        {
            var lines = string.Join(Environment.NewLine, results.Select(m => m.ErrorMessage));
            ComplexDialogs.ShowErrorOk(lines, "Some settings were invalid", "Invalid folders will remain configured, but will be disabled. You can delete them from the settings window");
            settings.Fix();
            SettingsProvider.SaveSettings(settings);
        }
        AutoMonitor.Start(settings.Settings);

        //Scan for webp files at startup, with graceful cancellation on application exit
        var cts = new CancellationTokenSource();
        Application.ApplicationExit += delegate { cts.Cancel(); };
        AutoMonitor.AutoScan(settings.Settings, cts.Token);

        //Show settings form if no settings exist
        if (settings.Settings.Length == 0)
        {
            ShowSettingsForm();
        }
        DuplicateCheck.PipeMessage += delegate
        {
            showForm = true;
            MessageBroker.SendMainFormShowEvent();
        };
    }

    private static void ShowSettingsForm()
    {
        showForm = false;
        var frm = Application.OpenForms.OfType<FrmSettings>().FirstOrDefault() ?? new FrmSettings();
        frm.Show();
        frm.BringToFront();
        frm.Focus();
    }
}
