using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursWork.Drive.DataAccess.Repositories;

public class FileDriveRepository : IFileDriveRepository
{
    private IDbContextFactory<DriveContext> _dbContextFactory;

    public FileDriveRepository(IDbContextFactory<DriveContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<FileDrive> AddAsync(FileDrive fileDrive)
    {

        using var context = await _dbContextFactory.CreateDbContextAsync();

        context.FileDrives.Add(fileDrive);
        await context.SaveChangesAsync();

        return fileDrive;
    }

    public async Task DeleteAsync(FileDrive fileDrive)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.FileDrives.Where(file => file.Id.Equals(fileDrive.Id)).ExecuteDeleteAsync();
    }

    public async Task<FileDrive?> GetByIdAsync(int id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.FileDrives
            .AsNoTracking()
            .FirstOrDefaultAsync(file => file.Id.Equals(id));
    }

    public async Task<List<FileDrive>> GetFilesAsync(int userId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.FileDrives
            .AsNoTracking()
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
        using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.FileDrives.Where(file => file.Id.Equals(fileDrive.Id)).ExecuteUpdateAsync(setters => setters.SetProperty(file => file.Name, fileDrive.Name));

        return fileDrive;
    }
}
