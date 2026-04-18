using System.IO.Pipes;
using System.Text;

namespace WebpKiller;

internal delegate void PipeMessageEventHandler(string message);


internal static class DuplicateCheck
{
    public static event PipeMessageEventHandler PipeMessage = delegate { };

    private static NamedPipeServerStream? server;
    private static readonly string pipeName = $"{Environment.UserName}-webp-killer";

    public static bool Init()
    {
        try
        {
            //If we can open the server pipe then we're the first instance
            server = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.CurrentUserOnly | PipeOptions.Asynchronous);
            server.BeginWaitForConnection(OnClient, null);
            return true;
        }
        catch
        {
            //Failed to open pipe, try to connect to it and inform it of the start attempt
            try
            {
                using var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.CurrentUserOnly | PipeOptions.Asynchronous);
                client.Connect(1000);
                client.Write("start"u8);
            }
            catch
            {
                //Failed to connect to the server. It might be stuck.
                //Allow to run a new instance
                return true;
            }
        }
        return false;
    }

    private static async void OnClient(IAsyncResult ar)
    {
        var buffer = new byte[5];
        var s = server;
        if (s == null)
        {
            return;
        }
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            s.EndWaitForConnection(ar);
            await s.ReadExactlyAsync(buffer, cts.Token);
            var str = Encoding.UTF8.GetString(buffer);
            if (str == "start")
            {
                PipeMessage(str);
            }
        }
        catch
        {

        }
        finally
        {
            s.Disconnect();
            s.BeginWaitForConnection(OnClient, null);
        }
    }
}
