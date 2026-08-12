using InputWeb.Domain.Entities;

namespace InputWeb.Domain.Interfaces;

public interface IRecordRepository
{
    Task<List<Recording>> GetRecords(string search = "");
    Task<Recording> GetRecordingById(Guid id);
    Recording CreateRecording(Recording recording);
    Task DeleteRecording(Guid id);

}