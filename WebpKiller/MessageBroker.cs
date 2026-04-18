using System.Runtime.InteropServices;

namespace WebpKiller;

/// <summary>
/// The message broker is used to send messages to other WebpKiller executables
/// in a way that processes them on the main thread
/// </summary>
internal static class MessageBroker
{
    private const int HWND_BROADCAST = 0xFFFF;

    [DllImport("user32.dll")]
    private static extern int RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport("user32.dll")]
    private static extern int PostMessage(int hWnd, int msg, int wParam, int lParam);

    private static int messageId;

    /// <summary>
    /// Registers a custom message used for main thread synchronization
    /// </summary>
    /// <remarks>
    /// Usually there is no need to invoke this method manually.
    /// It is safe to call it multiple times.
    /// This method is called automatically by <see cref="SendMainFormShowEvent"/> if not yet called manually
    /// </remarks>
    public static void Init()
    {
        if (messageId == 0)
        {
            messageId = RegisterWindowMessage("webp-killer-start-event");
        }
    }

    /// <summary>
    /// Sends a request to show the settings form to all WebpKiller processes
    /// </summary>
    /// <returns>true if message was sent, false otherwise</returns>
    /// <remarks>
    /// This automatically invokes <see cref="Init"/>
    /// if this has not yet been done manually
    /// </remarks>
    public static bool SendMainFormShowEvent()
    {
        if (messageId == 0)
        {
            Init();
        }
        return 0 != PostMessage(HWND_BROADCAST, messageId, 0x0, 0x0);
    }
}
