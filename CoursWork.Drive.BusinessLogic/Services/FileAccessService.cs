using AutoMapper;
using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.BusinessLogic.Exceptions;
using CoursWork.Drive.DataAccess.Repositories;
using CoursWork.Drive.Shared.Requests;
using FileAccess = CoursWork.Drive.DataAccess.Entities.FileAccess;

namespace CoursWork.Drive.BusinessLogic.Services;

public class FileAccessService : IFileAccessService
{
    private readonly IFileAccessRepository _fileAccessRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public FileAccessService(IMapper mapper, IFileAccessRepository fileAccessRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _fileAccessRepository = fileAccessRepository;
        _userRepository = userRepository;
    }

    public async Task<List<FileAccessDto>> AddAsync(FileAccessRequest fileRequest)
    {
        var ids = await _userRepository.GetIdByEmailAsync(fileRequest.Emails);
        var fileRequests = new List<FileAccess>();

        if (ids == null || ids.Count == 0)
        {
            throw new NotFoundException("Check the email address it where not found");
        }

        foreach (var id in ids)
        {
            var request = new FileAccess()
            {
                FileDriveId = fileRequest.FileId,
                UserId = id,
                AccessLevel = fileRequest.AccessLevel
            };

            if (!await _fileAccessRepository.ExistAsync(request))
            {
                fileRequests.Add(request);
            }
        }

        await _fileAccessRepository.AddAsync(fileRequests);   

        return _mapper.Map<List<FileAccessDto>>(fileRequests);
    }

    public async Task<FileAccessDto> DeleteAsync(int id)
    {
        var fileAccess = await _fileAccessRepository.GetByIdAsync(id);

        if(fileAccess is null)
        {
            throw new NotFoundException("this file access does not exist");
        }

        await _fileAccessRepository.DeleteAsync(fileAccess);

        return _mapper.Map<FileAccessDto>(fileAccess);
    }

    public async Task<List<FileDto>> GetByUserIdAsync(int userId)
    {
        var fileAccess = await _fileAccessRepository.GetByUserIdAsync(userId);

        return _mapper.Map<List<FileDto>>(fileAccess);
    }

    public async Task<FileAccessDto> UpdateAsync(FileAccessRequest fileRequest)
    {
        var fileAccess = await _fileAccessRepository.GetByIdAsync(fileRequest.Id);

        if (fileAccess is null)
        {
            throw new NotFoundException("this file access does not exist");
        }

        _mapper.Map(fileRequest, fileAccess);
        await _fileAccessRepository.UpdateAsync(fileAccess);

        return _mapper.Map<FileAccessDto>(fileAccess);
    }
}
