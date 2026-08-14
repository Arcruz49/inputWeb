using InputWeb.Application.Interfaces;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;

namespace InputWeb.Application.UseCases;

public class CreateRecordingUseCase(IUnitOfWork unitOfWork, IRecordRepository recordRepository, IFileStorage fileStorage) : ICreateRecordingUseCase
{
    public async Task<Guid> ExecuteAsync(Guid userId,
        string projectName,
        Stream videoStream,
        Stream eventsStream)
    {
        var recordingId = Guid.NewGuid();

        var videoUrl = await fileStorage.UploadAsync(videoStream, $"{recordingId}/video.mp4", "video/mp4");
        var eventsUrl = await fileStorage.UploadAsync(eventsStream, $"{recordingId}/events.txt", "text/plain");

        var recording = new Recording()
        {
            Id = recordingId,
            UserId = userId,
            ProjectName = projectName,
            CreatedAt = DateTime.UtcNow,
            VideoUrl = videoUrl,
            EventsUrl = eventsUrl
        };

        recordRepository.CreateRecording(recording);
        await unitOfWork.SaveChangesAsync();

        return recordingId;
    }
}