using CoursWork.Drive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursWork.Drive.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DriveContext _driveContext;

    public UserRepository(DriveContext driveContext)
    {
        _driveContext = driveContext;
    }

    public async Task<User> AddAsync(User user)
    {
        _driveContext.Users.Add(user);
        await _driveContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken)
    {
        var userLooked = await _driveContext.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Where(user => user.Email == email && user.Password == password)
            .FirstOrDefaultAsync();

        return userLooked;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _driveContext.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

}
