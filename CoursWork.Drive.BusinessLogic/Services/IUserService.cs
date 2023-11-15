using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.BusinessLogic.Services;

public interface IUserService
{
    public Task<UserDto> RegisterAsync(UserRegister userRegister, CancellationToken cancellationToken);
}
