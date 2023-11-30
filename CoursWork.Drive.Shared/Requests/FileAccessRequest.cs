namespace CoursWork.Drive.Shared.Requests;

public class FileAccessRequest
{
    public int Id { get; set; }
    public List<string> Emails { get; set; }
    public int FileId { get; set; }
    public AccessLevel AccessLevel { get; set; }
}
