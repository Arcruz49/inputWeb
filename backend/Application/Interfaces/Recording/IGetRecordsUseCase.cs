using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IGetRecordsUseCase
{
    Task<List<RecordResponse>> ExecuteAsync();
}