using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.BusinessLogic.Services;

public interface IFileDriveService
{
    public Task<FileDto> AddAsync(FileRequest fileRequest);
    public Task<FileDto> RenameAsync(FileRequest fileRequest);
    public Task<string> DeleteAsync(int id);
    public Task<List<FileDto>> GetFilesAsync(int userId);
    public Task<FileDto> GetByIdAsync(int id);
}
