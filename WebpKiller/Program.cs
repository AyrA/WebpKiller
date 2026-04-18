using System.Diagnostics;
using WebpKiller;
using WebpKiller.Settings;

internal class Program
{
    private static NotifyIcon? icon = null;
    private static readonly Stopwatch cooldown = Stopwatch.StartNew();
    private static bool showForm = false;
    private static bool hasInit = false;

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

        if (!Converter.HasMagick())
        {
            ComplexDialogs.ShowErrorOk("This application requires ImageMagick to function properly, which is either not installed, or not contained in your PATH environment variable. Please install imagemagick, then start this application again.", "ImageMagick not found", "ImageMagick is a free and open source image conversion and edit tool. You can download ImageMagick from <a href=\"https://imagemagick.org/download/\">their website</a>. The site tries to automatically detect the best download for you, and thus the first download link is usually the best choice.");
            Application.Exit();
            return;
        }

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
