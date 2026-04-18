using System.Diagnostics;

namespace WebpKiller;

/// <summary>
/// Provides templates for more complex dialogs
/// </summary>
internal static class ComplexDialogs
{
    /// <summary>
    /// Shows dialog with a Yes/No button, and a "Show help" expander
    /// </summary>
    /// <param name="message">Main message</param>
    /// <param name="title">Title</param>
    /// <param name="help">Help text</param>
    /// <returns>true if "Yes" was clicked, false if "No" was clicked</returns>
    public static bool ShowHelpYesNo(string message, string title, string help)
    {
        var page = new TaskDialogPage
        {
            AllowCancel = false,
            AllowMinimize = false,
            Caption = title,
            Heading = title,
            Icon = TaskDialogIcon.Information,
            Text = message,
            Expander = new TaskDialogExpander(help)
        };
        page.Expander.ExpandedButtonText = "Hide help";
        page.Expander.CollapsedButtonText = "Show help";
        page.Buttons.Add(TaskDialogButton.Yes);
        page.Buttons.Add(TaskDialogButton.No);
        return TaskDialog.ShowDialog(page) == TaskDialogButton.Yes;
    }

    /// <summary>
    /// Shows an error dialog.
    /// Returns when the dialog is closed
    /// </summary>
    /// <param name="message">Main message</param>
    /// <param name="title">Error title</param>
    /// <param name="help">Text containing further help</param>
    public static void ShowErrorOk(string message, string title, string help)
    {
        var page = new TaskDialogPage
        {
            AllowCancel = true,
            AllowMinimize = false,
            EnableLinks = true,
            Caption = title,
            Heading = title,
            Icon = TaskDialogIcon.Error,
            Text = message,
            Expander = new TaskDialogExpander(help)
        };
        page.Expander.ExpandedButtonText = "Hide help";
        page.Expander.CollapsedButtonText = "Show help";
        page.Buttons.Add(TaskDialogButton.OK);
        page.LinkClicked += (sender, args) =>
        {
            var psi = new ProcessStartInfo()
            {
                FileName = args.LinkHref,
                UseShellExecute = true
            };
            Process.Start(psi)?.Dispose();
            page.BoundDialog?.Close();
        };
        TaskDialog.ShowDialog(page);
    }
}
