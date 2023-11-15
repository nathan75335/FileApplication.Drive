using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursWork.Drive.DataAccess.Repositories;

public class FileDriveRepository : IFileDriveRepository
{
    private readonly DriveContext _driveContext;
    private readonly DbSet<FileDrive> _files;

    public FileDriveRepository(DriveContext driveContext)
    {
        _driveContext = driveContext;
        _files = _driveContext.Set<FileDrive>();
    }

    public async Task<FileDrive> AddAsync(FileDrive fileDrive)
    {

        _files.Add(fileDrive);
        await _driveContext.SaveChangesAsync();

        return fileDrive;
    }

    public async Task DeleteAsync(FileDrive fileDrive)
    {
        await _files.Where(file => file.Id.Equals(fileDrive.Id)).ExecuteDeleteAsync();
    }

    public async Task<FileDrive?> GetByIdAsync(int id)
    {
        return await _files
            .AsNoTracking()
            .FirstOrDefaultAsync(file => file.Id.Equals(id));
    }

    public async Task<List<FileDrive>> GetFilesAsync(int userId)
    {
        return await _files
            .Where(file => file.UserId == userId).Select(source => new FileDrive
            {
                Id = source.Id,
                Name = source.Name,
                Size = source.Size,
                Description = source.Description,
                UserId = source.UserId,
                Extension = source.Extension
            }).ToListAsync();
    }

    public async Task<FileDrive> UpdateAsync(FileDrive fileDrive)
    {
        await _files.Where(file => file.Id.Equals(fileDrive.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(file => file.Name, fileDrive.Name));

        return fileDrive;
    }
}
