using AutoMapper;
using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.BusinessLogic.Exceptions;
using CoursWork.Drive.DataAccess.Entities;
using CoursWork.Drive.DataAccess.Repositories;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> RegisterAsync(UserRegister userRegister, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(userRegister.Email, cancellationToken);

        if(user is not null)
        {
            throw new ExistException("A user with this email already exist in the system");
        }


        var userAdd = _mapper.Map<User>(userRegister);
        userAdd.RoleId = -1;
        return _mapper.Map<UserDto>(await _userRepository.AddAsync(userAdd));
    }
}
