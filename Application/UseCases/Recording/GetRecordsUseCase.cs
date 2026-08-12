using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;

namespace InputWeb.Application.UseCases;

public class GetRecordsUseCase(IRecordRepository recordRepository
    // arrumar uma forma de pegar os arquivos :((( !!
    // , IFileStorage fileStorage
    ) : IGetRecordsUseCase
{
    public async Task<List<RecordResponse>> ExecuteAsync()
    {
        var records = await recordRepository.GetRecords();
        
        return records.Select(record => new RecordResponse(
            record.Id,
            record.ProjectName,
            record.User.Name,
            record.CreatedAt
        )).ToList();
    }
}