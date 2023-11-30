using CoursWork.Drive.Shared;

namespace CoursWork.Drive.BusinessLogic.DTO_s;

public class FileAccessDto
{
    public int Id { get; set; }
    public FileDto? FileDrive { get; set; }
    public UserDto? User { get; set; }
    public AccessLevel AccessLevel { get; set; } = AccessLevel.None;
}
