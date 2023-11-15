using AutoMapper;
using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.DataAccess.Entities;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.Server.Profiles;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<UserRegister, User>()
            .ReverseMap();

        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<FileRequest, FileDrive>()
            .ReverseMap();
        CreateMap<FileDto, FileDrive>()
            .ForMember(source => source.Content, dest => dest.Ignore())
            .ReverseMap();
    }
}
