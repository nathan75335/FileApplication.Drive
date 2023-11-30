using CoursWork.Drive.BusinessLogic.Services;
using CoursWork.Drive.Shared;
using CoursWork.Drive.Shared.JsonHanlerRequestResponse;
using CoursWork.Drive.Shared.Requests;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using CoursWork.Drive.BusinessLogic.DTO_s;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace CoursWork.Drive.Server.MessageHandler;

public class JsonMessageHandler
{
    private readonly IFileDriveService _fileDriveService;
    private readonly WebSocketServerConnectionManager _connectionManager;
    private readonly IConfiguration _configuration;
    private readonly IFileAccessService _fileAccessService;

    public JsonMessageHandler(IFileDriveService fileDriveService,
        WebSocketServerConnectionManager connectionManager,
        IConfiguration configuration,
        IFileAccessService fileAccessService)
    {
        _fileDriveService = fileDriveService;
        _connectionManager = connectionManager;
        _configuration = configuration;
        _fileAccessService = fileAccessService;
    }

    public async Task HandleJsonMessageAsync(JsonRequest jsonRequest, WebSocket webSocket)
    {
        var isValidated = ValidateToken(jsonRequest.Token);
        if (!isValidated)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", default);
        }

        if (jsonRequest is {RequestType: RequestType.Request } && isValidated)
        {
            FileRequest? fileRequest = null;
            FileAccessRequest? fileAccessRequest = null;
            JsonRequest? response = new JsonRequest();
            string repsonseSerializedAll = string.Empty;
            var requestAllFiles = new List<BusinessLogic.DTO_s.FileDto>();

            switch (jsonRequest.MethodType)
            {
                #region File Crud Operations
                case MethodType.Post:
                    fileRequest = JsonConvert.DeserializeObject<FileRequest>(jsonRequest.Body);
                    await _fileDriveService.AddAsync(fileRequest);
                    var filesAfterPost = await _fileDriveService.GetFilesAsync(jsonRequest.UserId);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(filesAfterPost);
                    response.MethodType = MethodType.Get;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);
                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;
                case MethodType.Put:
                    fileRequest = JsonConvert.DeserializeObject<FileRequest>(jsonRequest.Body);
                    await _fileDriveService.RenameAsync(fileRequest);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(new Response { StatusCode = WebSocketStatusCodes.OK });
                    response.MethodType = MethodType.Put;
                    response.ConnectionId = jsonRequest.ConnectionId;

                    requestAllFiles = await _fileDriveService.GetFilesAsync(jsonRequest.UserId);
                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(requestAllFiles);
                    response.MethodType = MethodType.Get;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);

                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;
                case MethodType.Delete:
                    fileRequest = JsonConvert.DeserializeObject<FileRequest>(jsonRequest.Body);
                    await _fileDriveService.DeleteAsync(fileRequest.Id);

                    requestAllFiles = await _fileDriveService.GetFilesAsync(jsonRequest.UserId);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(requestAllFiles);
                    response.MethodType = MethodType.Get;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);
                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;
                case MethodType.Get:
                    var files = await _fileDriveService.GetFilesAsync(jsonRequest.UserId);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(files);
                    response.MethodType = MethodType.Get;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);
                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;
                case MethodType.GetOne:
                    fileRequest = JsonConvert.DeserializeObject<FileRequest>(jsonRequest.Body);
                    var file = await _fileDriveService.GetByIdAsync(fileRequest.Id);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(file);
                    response.MethodType = MethodType.GetOne;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    var repsonseSerialized = JsonConvert.SerializeObject(response);
                    await SendMessageAsync(webSocket, repsonseSerialized, WebSocketMessageType.Binary);
                    break;
                #endregion

                #region FileAccess Crud Operations
                case MethodType.GetFileAccessByUserId:
                    var filesAccessRequest = await _fileAccessService.GetByUserIdAsync(jsonRequest.UserId);

                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(filesAccessRequest);
                    response.MethodType = MethodType.GetFileAccessByUserId;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);
                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;
                case MethodType.PostFileAccess:
                    fileAccessRequest = JsonConvert.DeserializeObject<FileAccessRequest>(jsonRequest.Body);
                    await _fileAccessService.AddAsync(fileAccessRequest);

                    //response.RequestType = RequestType.Response;
                    //response.Body = JsonConvert.SerializeObject(new Response { StatusCode = WebSocketStatusCodes.OK });
                    //response.MethodType = MethodType.PostFileAccess;
                    //response.ConnectionId = jsonRequest.ConnectionId;

                    requestAllFiles = await _fileAccessService.GetByUserIdAsync(jsonRequest.UserId);
                    response.RequestType = RequestType.Response;
                    response.Body = JsonConvert.SerializeObject(requestAllFiles);
                    response.MethodType = MethodType.GetFileAccessByUserId;
                    response.ConnectionId = jsonRequest.ConnectionId;
                    repsonseSerializedAll = JsonConvert.SerializeObject(response);

                    await SendMessageAsync(repsonseSerializedAll, WebSocketMessageType.Binary, jsonRequest.UserId);
                    break;

                #endregion

            }
        }
    }

    public async Task SendMessageAsync(string message, WebSocketMessageType websocketMessageType, int userId)
    {
        var buffer = Encoding.UTF32.GetBytes(message);
        var sockets = _connectionManager.GetUserConnections(userId);
        
        foreach (var socket in sockets)
        {
            if(socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(buffer, websocketMessageType, true, CancellationToken.None);
            }
        }
    }

    public async Task SendMessageAsync(WebSocket webSocket, string message, WebSocketMessageType websocketMessageType)
    {
        var buffer = Encoding.UTF32.GetBytes(message);
        await webSocket.SendAsync(buffer, websocketMessageType, true, CancellationToken.None);
    }

    public  bool ValidateToken(string authToken) // Retrieve token from request header
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = this.GetValidationParameters();

        SecurityToken validatedToken;
        try
        {
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            Thread.CurrentPrincipal = principal;
        }catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidIssuer = _configuration["JWT:Issuer"],
            ValidAudience = _configuration["JWT:Audience"],
            RequireExpirationTime = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true
        };
    }
}
