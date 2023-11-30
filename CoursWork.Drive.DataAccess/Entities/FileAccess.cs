using CoursWork.Drive.Shared;

namespace CoursWork.Drive.DataAccess.Entities;

public class FileAccess
{
    public int Id { get; set; }
    public int FileDriveId { get; set; }
    public FileDrive? FileDrive { get; set; }
    public int UserId { get; set; }
    public User? User {  get; set; }
    public AccessLevel AccessLevel { get; set; } = AccessLevel.None;
}
