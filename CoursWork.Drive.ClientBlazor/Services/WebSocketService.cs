using System.Net.WebSockets;

namespace CoursWork.Drive.ClientBlazor.Services;

public class WebSocketService
{
    private readonly ClientWebSocket _clientWebSocket;

    public WebSocketService()
    {
        _clientWebSocket = new ClientWebSocket();
    }

    public async Task ConnectAsync(string url, string jwtToken)
    {
        _clientWebSocket.Options.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
        await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
    }
}
