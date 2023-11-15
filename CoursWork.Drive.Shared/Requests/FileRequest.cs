namespace CoursWork.Drive.Shared.Requests;

public class FileRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Extension { get; set; }
    public byte[] Content { get; set; }
    public int UserId { get; set; }
    public double Size { get; set; }

}
