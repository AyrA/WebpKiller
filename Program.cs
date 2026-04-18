using System.Diagnostics;
using WebpKiller;
using WebpKiller.Settings;

internal class Program
{
    private static NotifyIcon? icon = null;
    private static readonly Stopwatch cooldown = Stopwatch.StartNew();

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
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Idle += Init;
        Application.Run();
        AutoMonitor.Stop();
        icon?.Dispose();
    }

    private static void Init(object? sender, EventArgs e)
    {
        Application.Idle -= Init;

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
        AutoMonitor.AutoScan(settings.Settings);
        if (settings.Settings.Length == 0)
        {
            ShowSettingsForm();
        }
    }

    private static void ShowSettingsForm()
    {
        var frm = Application.OpenForms.OfType<FrmSettings>().FirstOrDefault() ?? new FrmSettings();
        frm.Show();
        frm.BringToFront();
        frm.Focus();
    }
}
