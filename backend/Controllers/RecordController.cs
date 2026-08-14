using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using InputWeb.Application.DTOs.Request;
using InputWeb.Application.Interfaces;
using InputWeb.Application.UseCases;

namespace InputWeb.Controllers;

[Authorize]
[Route("Record")]
public class RecordController(ICreateRecordingUseCase createRecordingUseCase, IGetRecordByIdUseCase getRecordByIdUseCase,
    IGetRecordsUseCase getRecordsUseCase, IGenerateDownloadUseCase generateDownloadUseCase, IDeleteRecordingUseCase deleteRecordingUseCase) : BaseController
{
    // [EnableRateLimiting("")]
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] RecordingDTO request)
    {
        await using var videoStream = request.Video.OpenReadStream();
        await using var eventsStream = request.Events.OpenReadStream();

        var id = await createRecordingUseCase.ExecuteAsync(UserId, request.ProjectName, videoStream, eventsStream);

        return Ok(new { recordingId = id });
    }

    // [EnableRateLimiting("register")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecordById(Guid id)
    {
        var result = await getRecordByIdUseCase.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetRecords()
    {
        var result = await getRecordsUseCase.ExecuteAsync();
        return Ok(result);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> GetDownloadLinks(Guid id)
    {
        var response = await generateDownloadUseCase.ExecuteAsync(id);
        return Ok(new { response.url_video, response.url_events});
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteRecord(Guid id)
    {
        await deleteRecordingUseCase.ExecuteAsync(id);    
        return NoContent();
    }
}