using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;

namespace InputWeb.Application.UseCases;

public class GetRecordByIdUseCase(IRecordRepository recordRepository
    // arrumar uma forma de pegar os arquivos :((( !!
    // , IFileStorage fileStorage
    ) : IGetRecordByIdUseCase
{
    public async Task<RecordResponse> ExecuteAsync(Guid recordId)
    {
        var record = await recordRepository.GetRecordingById(recordId);
        
        return new RecordResponse(record.Id, record.ProjectName, record.User.Name, record.CreatedAt);
    }
}