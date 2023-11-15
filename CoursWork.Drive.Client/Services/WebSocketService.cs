using System.Net.WebSockets;

namespace CoursWork.Drive.Client.Services;

public class WebSocketService
{
    public ClientWebSocket _clientWebSocket { get; set; }

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
