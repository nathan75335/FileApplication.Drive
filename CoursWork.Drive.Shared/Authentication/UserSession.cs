namespace CoursWork.Drive.Shared.Authentication;

public class UserSession
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
    public int ExpiresInMinutes { get; set;}
    public DateTime ExpiryTimeStamp { get; set; }
}
