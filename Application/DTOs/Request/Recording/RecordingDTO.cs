namespace InputWeb.Application.DTOs.Request;

public class RecordingDTO
{
    public string ProjectName { get; set; } = string.Empty;
    public IFormFile Video { get; set; } = null!;
    public IFormFile Events { get; set; } = null!;
}