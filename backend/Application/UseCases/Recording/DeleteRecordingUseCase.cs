using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;

namespace InputWeb.Application.UseCases;

public class DeleteRecordingUseCase(IRecordRepository recordRepository, IFileStorage fileStorage, IUnitOfWork unitOfWork
    ) : IDeleteRecordingUseCase
{
    public async Task ExecuteAsync(Guid recordId)
    {
        await recordRepository.DeleteRecording(recordId);
        await unitOfWork.SaveChangesAsync();

        var videoBlobName = $"{recordId}/video.mp4";
        var eventBlobName = $"{recordId}/events.txt";

        await fileStorage.DeleteAsync(videoBlobName);
        await fileStorage.DeleteAsync(eventBlobName);

        return;
    }
}