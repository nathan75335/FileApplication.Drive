using System;
using System.Net.WebSockets;

namespace CoursWork.Drive.ClientBlazor.Services;

public class WebSocketService
{
    private readonly ClientWebSocket _clientWebSocket;
    
    public ClientWebSocket ClientWebSocket
    {
        get 
        { 
            return _clientWebSocket; 
        }
    }
    public  WebSocketService()
    {
        _clientWebSocket = new ClientWebSocket();
    }

    public async Task ConnectAsync(string url)
    {
        await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
    }

}
