namespace InputWeb.Application.DTOs.Responses;

public record RecordResponse(
    Guid id,
    string project_name,
    string user_name,
    DateTime created_at
);