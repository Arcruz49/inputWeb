using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IGenerateDownloadUseCase
{
    Task<UrlFilesRecordResponse> ExecuteAsync(Guid recordId);
}