using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.BusinessLogic.Services;

public interface IFileAccessService
{
    Task<List<FileAccessDto>> AddAsync(FileAccessRequest fileRequest);
    Task<FileAccessDto> UpdateAsync(FileAccessRequest fileRequest);
    Task<FileAccessDto> DeleteAsync(int id);
    Task<List<FileDto>> GetByUserIdAsync(int userId);
    
}