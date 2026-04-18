using System.Runtime.InteropServices;

namespace WebpKiller;

internal static class MessageBroker
{
    private const int HWND_BROADCAST = 0xFFFF;

    [DllImport("user32.dll")]
    private static extern int RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport("user32.dll")]
    private static extern int PostMessage(int hWnd, int msg, int wParam, int lParam);

    private static int messageId;

    public static void Init()
    {
        if (messageId == 0)
        {
            messageId = RegisterWindowMessage("webp-killer-start-event");
        }
    }

    public static bool SendMainFormShowEvent()
    {
        if (messageId == 0)
        {
            Init();
        }
        return 0 != PostMessage(HWND_BROADCAST, messageId, 0x0, 0x0);
    }
}
