using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IGetRecordByIdUseCase
{
    Task<RecordResponse> ExecuteAsync(Guid RecordId);
}