using CoursWork.Drive.BusinessLogic.Services;
using CoursWork.Drive.Server.MessageHandler;
using CoursWork.Drive.Shared.JsonHanlerRequestResponse;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace CoursWork.Drive.Server.Middlewares;

public class WebSocketServerMiddleware
{
    public readonly RequestDelegate _next;
    private readonly WebSocketServerConnectionManager _connectionManager;

    public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerConnectionManager connectionManager)
    {
        _next = next;
        _connectionManager = connectionManager;

    }

    public async Task Invoke(HttpContext context, JsonMessageHandler jsonMessageHandler)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await _next(context);
        }

        if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/ws")
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string connectionId = _connectionManager.AddWebSocket(webSocket);
            await jsonMessageHandler.SendMessageAsync(webSocket, connectionId, WebSocketMessageType.Text);

            await ReceiveAsync(webSocket, async (result, buffer) =>
            {
                string message = Encoding.UTF32.GetString(buffer, 0, buffer.Length);
                JsonRequest? messageDeserialized = JsonConvert.DeserializeObject<JsonRequest>(message);

                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    if (messageDeserialized.UserId != 0)
                    {
                        string connectionIdFromUser = !string.IsNullOrEmpty(connectionId) ? connectionId : messageDeserialized.ConnectionId;
                        _connectionManager.AddUserIdToConnection(messageDeserialized.UserId, connectionIdFromUser);
                    }

                    await jsonMessageHandler.HandleJsonMessageAsync(messageDeserialized, webSocket);
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _connectionManager.RemoveWebSocketAsync(messageDeserialized.ConnectionId, CancellationToken.None);
                }
            });
        }
    }

    private async Task ReceiveAsync(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        int bufferSize = 1024; // Initial buffer size
        byte[] buffer = new byte[bufferSize];
        MemoryStream memoryStream = new MemoryStream();

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                memoryStream.Write(buffer, 0, result.Count);

                if (!result.EndOfMessage)
                {
                    continue; // Wait for the next chunk
                }

                byte[] message = memoryStream.ToArray();
                handleMessage(result, message);
                memoryStream.SetLength(0); // Reset memory stream for the next message
            }

            // If the buffer is too small, double its size
            if (result.Count == bufferSize)
            {
                bufferSize *= 2;
                Array.Resize(ref buffer, bufferSize);
            }
        }
    }

}
