using FileAccess =  CoursWork.Drive.DataAccess.Entities.FileAccess;
using Microsoft.EntityFrameworkCore;
using CoursWork.Drive.DataAccess.Entities;

namespace CoursWork.Drive.DataAccess.Repositories;

public class FileAccessRepository : IFileAccessRepository
{
    private IDbContextFactory<DriveContext> _dbContextFactory;

    public FileAccessRepository(IDbContextFactory<DriveContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<FileAccess> AddAsync(FileAccess userFile)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        context.FileAccesses.Add(userFile);
        await context.SaveChangesAsync();

        return userFile;
    }

    public async Task<List<Entities.FileAccess>> AddAsync(List<FileAccess> userFiles)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        await context.FileAccesses.AddRangeAsync(userFiles);
        await context.SaveChangesAsync();

        return userFiles;
    }

    public async Task<int> DeleteAsync(FileAccess userFile)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        context.FileAccesses.Remove(userFile);
        var result = await context.SaveChangesAsync();

        return result;
    }

    public async Task<FileAccess?> GetByIdAsync(int id)
    {
        using var context  = await _dbContextFactory.CreateDbContextAsync();

        return await context.FileAccesses.FirstOrDefaultAsync(access => access.Id == id); 
    }

    public async Task<List<FileDrive>> GetByUserIdAsync(int userId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var result = await context.FileAccesses
            .Include(x => x.FileDrive)
            .Include(x => x.User)
            .Where(x => x.UserId.Equals(userId))
            .Select(x => x.FileDrive)
            .ToListAsync();

        return result;
    }

    public async Task<FileAccess> UpdateAsync(FileAccess userFile)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        context.FileAccesses.Update(userFile);
        await context.SaveChangesAsync();

        return userFile;
    }

    public async Task<bool> ExistAsync(FileAccess userFile)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.FileAccesses
            .Where(fileAccess => fileAccess.UserId.Equals(userFile.UserId) && fileAccess.FileDriveId.Equals(userFile.FileDriveId))
            .AnyAsync();
    }
}
