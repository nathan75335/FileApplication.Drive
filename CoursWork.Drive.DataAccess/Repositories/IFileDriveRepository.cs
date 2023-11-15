using CoursWork.Drive.DataAccess.Entities;

namespace CoursWork.Drive.DataAccess.Repositories;

public interface IFileDriveRepository
{
    public Task<FileDrive> AddAsync(FileDrive fileDrive);
    public Task<FileDrive> UpdateAsync(FileDrive fileDrive);
    public Task DeleteAsync(FileDrive fileDrive);
    public Task<FileDrive?> GetByIdAsync(int id);
    public Task<List<FileDrive>> GetFilesAsync(int userId);
}
