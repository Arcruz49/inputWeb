using InputWeb.Domain.Entities;
using InputWeb.Domain.Exceptions;
using InputWeb.Domain.Interfaces;
using InputWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InputWeb.Infrastructure.Repositories;

public class RecordRepository(Context db) :IRecordRepository
{
    public async Task<List<Recording>> GetRecords(string search = "")
    {
        return await db.Recordings.AsNoTracking().Where(a => (a.ProjectName ?? "").Contains(search ?? "")).ToListAsync();
    }
    public async Task<Recording> GetRecordingById(Guid id)
    {
        return await db.Recordings.Where(a => a.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Registro não encontrado");
    }

    public Recording CreateRecording(Recording recording)
    {
        db.Recordings.Add(recording);
        return recording;
    }
    public async Task DeleteRecording(Guid id)
    {
        var recording = await GetRecordingById(id);
        db.Recordings.Remove(recording);
    }

}