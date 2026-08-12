namespace InputWeb.Application.Interfaces;

public interface ICreateRecordingUseCase
{
    Task<Guid> ExecuteAsync(Guid userId,
        string projectName,
        Stream videoStream,
        Stream eventsStream);
}