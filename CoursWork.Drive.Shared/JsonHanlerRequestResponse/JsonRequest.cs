namespace CoursWork.Drive.Shared.JsonHanlerRequestResponse;

public class JsonRequest
{
    public string ConnectionId { get; set; }
    public MethodType MethodType { get; set; }
    public string Body { get; set; }
    public RequestType RequestType { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
}
