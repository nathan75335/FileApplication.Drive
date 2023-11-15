using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace CoursWork.Drive.BusinessLogic.Services;

public class WebSocketServerConnectionManager
{
    private ConcurrentDictionary<string,WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
    private ConcurrentDictionary<int, List<string>>  _userConnections = new ConcurrentDictionary<int, List<string>>();

    public string AddWebSocket(WebSocket socket)
    {
        var connectionId = Guid.NewGuid().ToString();

        if (!_sockets.TryAdd(connectionId, socket))
        {
            Console.WriteLine("Could not add the socket connetion");

            throw new ArgumentException("Could not add the socket connetion");
        }

        Console.WriteLine($"Connection {connectionId} was added succesfully");

        return connectionId;
    }

    public async Task RemoveWebSocketAsync(string connectionId, CancellationToken cancellationToken)
    {
        var isClosed = _sockets.Remove(connectionId, out var socketClosed); ;

        if(isClosed)
        {
            await socketClosed.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection Closed", cancellationToken);
        }
    }

    public string AddUserIdToConnection(int userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var list))
        {
            
            list.Add(connectionId);
            _userConnections[userId] = list;
        }

        list = new List<string> { connectionId };
        _userConnections.TryAdd(userId, list);

        Console.WriteLine($"Adde the connection id {connectionId} to the user {userId}");

        return connectionId;
    }

    public ConcurrentDictionary<string, WebSocket> GetSockets()
    {
        return _sockets;
    }

    public List<WebSocket> GetUserConnections(int userId)
    {
       var result = _userConnections.TryGetValue(userId, out var userConnections);

        var sockets =  new List<WebSocket>();

        if(userConnections is null)
        {
            userConnections = new List<string>();
        }

        foreach(var connection in userConnections)
        {
            sockets.Add(_sockets[connection]);
        }

        return sockets;
    }
}
