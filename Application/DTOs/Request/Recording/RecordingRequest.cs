namespace InputWeb.Application.DTOs.Request;
public class RecordingRequest()
{
    public Guid UserId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Stream? VideoStream { get; set; }
    public Stream? EventsStream { get; set; }
}