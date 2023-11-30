using CoursWork.Drive.DataAccess.Entities;

namespace CoursWork.Drive.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    public Task<User?> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken);
    public Task<User> AddAsync(User user);
    public Task<List<int>> GetIdByEmailAsync(List<string> emails);
}
