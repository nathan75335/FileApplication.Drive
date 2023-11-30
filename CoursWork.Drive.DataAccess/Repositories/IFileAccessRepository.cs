using CoursWork.Drive.DataAccess.Entities;
using FileAccess = CoursWork.Drive.DataAccess.Entities.FileAccess;

namespace CoursWork.Drive.DataAccess.Repositories;

public interface IFileAccessRepository
{
    Task<FileAccess> AddAsync(FileAccess userFile);
    Task<FileAccess> UpdateAsync(FileAccess userFile);
    Task<int> DeleteAsync(FileAccess userFile);
    Task<List<FileDrive>> GetByUserIdAsync(int userId);
    Task<FileAccess> GetByIdAsync(int id);
    Task<List<FileAccess>> AddAsync(List<FileAccess> userFiles);
    Task<bool> ExistAsync(FileAccess userFile);
}
