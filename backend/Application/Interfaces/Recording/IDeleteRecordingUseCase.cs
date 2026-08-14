using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IDeleteRecordingUseCase
{
    Task ExecuteAsync(Guid recordId);
}