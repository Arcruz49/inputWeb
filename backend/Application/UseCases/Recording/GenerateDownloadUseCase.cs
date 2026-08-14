using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Exceptions;
using InputWeb.Domain.Interfaces;

namespace InputWeb.Application.UseCases;

public class GenerateDownloadUseCase(IRecordRepository recordRepository, IFileStorage fileStorage
    ) : IGenerateDownloadUseCase
{
    public async Task<UrlFilesRecordResponse> ExecuteAsync(Guid recordId)
    {
        var record = await recordRepository.GetRecordingById(recordId) ?? throw new NotFoundException("Registros não encontrados");

        var validFor = TimeSpan.FromMinutes(15);
        var videoUrl = fileStorage.GenerateDownloadUrl($"{recordId}/video.mp4", validFor);
        var eventsUrl = fileStorage.GenerateDownloadUrl($"{recordId}/events.txt", validFor);
        
        return new UrlFilesRecordResponse(record.Id, videoUrl, eventsUrl);
    }
}