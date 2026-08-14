namespace InputWeb.Application.DTOs.Responses;

public record UrlFilesRecordResponse(
    Guid id,
    string url_video,
    string url_events
);