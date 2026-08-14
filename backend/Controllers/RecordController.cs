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
    IGetRecordsUseCase getRecordsUseCase, IFileStorage fileStorage) : BaseController
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
        var record = await getRecordByIdUseCase.ExecuteAsync(id);
        if (record is null) return NotFound();

        var validFor = TimeSpan.FromMinutes(15);
        var videoUrl = fileStorage.GenerateDownloadUrl($"{id}/video.mp4", validFor);
        var eventsUrl = fileStorage.GenerateDownloadUrl($"{id}/events.txt", validFor);

        return Ok(new { videoUrl, eventsUrl });
    }
}