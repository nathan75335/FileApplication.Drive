using AutoMapper;
using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.BusinessLogic.Exceptions;
using CoursWork.Drive.DataAccess.Entities;
using CoursWork.Drive.DataAccess.Repositories;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.BusinessLogic.Services;

public class FileDriveService : IFileDriveService
{
    private readonly IMapper _mapper;
    private readonly IFileDriveRepository _fileRepository;

    public FileDriveService(IMapper mapper, IFileDriveRepository fileDriveRepository)
    {
        _mapper = mapper;
        _fileRepository = fileDriveRepository;
    }

    public async Task<FileDto> AddAsync(FileRequest fileRequest)
    {
        return  _mapper.Map<FileDto>(await _fileRepository.AddAsync(_mapper.Map<FileDrive>(fileRequest)));
    }

    public async Task<string> DeleteAsync(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);

        if(file is null)
        {
            throw new NotFoundException("This file does not exist");
        }

        await _fileRepository.DeleteAsync(file);

        return "The file has been deleted";
    }

    public async Task<FileDto> GetByIdAsync(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);

        if(file is null )
        {
            throw new NotFoundException("This file does not exist");
        }

        return _mapper.Map<FileDto>(file);
    }

    public async Task<List<FileDto>> GetFilesAsync(int userId)
    {
        return _mapper.Map<List<FileDto>>(await _fileRepository.GetFilesAsync(userId));
    }

    public async Task<FileDto> RenameAsync(FileRequest fileRequest)
    {
        var file = await _fileRepository.GetByIdAsync(fileRequest.Id);

        if(file is null)
        {
            throw new NotFoundException("This file does not exist");
        }

        file.Name = fileRequest.Name;
        await _fileRepository.UpdateAsync(file);

        return _mapper.Map<FileDto>(file);
    }
}
