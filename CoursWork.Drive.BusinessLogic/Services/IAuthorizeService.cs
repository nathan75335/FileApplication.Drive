using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.DataAccess.Entities;
using CoursWork.Drive.Shared.Authentication;

namespace CoursWork.Drive.BusinessLogic.Services;
/// <summary>
/// The auhtorization service to manage the authorization of users
/// </summary>
public interface IAuthorizeService
{
    /// <summary>
    /// Function to authorize the user to get token
    /// </summary>
    /// <param name="password">The password of the user</param>
    /// <returns>A <see cref="Task"/> That contains a <see cref="TokenDto"/></returns>
    Task<UserSession> AuthorizeAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Function to verify if the user is valid
    /// </summary>
    /// <param name="password">The password of the user</param>
    /// <returns>A <see cref="Task"/> That contains true if the user is valid and false in 
    /// the other case</returns>
    Task<User> ValidateUserAsync(string email, string password, CancellationToken cancellationToken);
}